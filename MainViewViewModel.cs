using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Windows;
using Tekla.Structures.Model;
using Tekla.Structures.Drawing;

namespace PlotNumbering
{
    public class MainViewViewModel
    {
        public DelegateCommand SetPlotNumbersCommand { get; }

        public String NumberingPrefix { get; set; } //Currently useless. Need an implementation
        public String StartNumber { get; set; }
        public String Step { get; set; }
        public String SelectedDrawingType { get; set; }

        public MainViewViewModel()
        {
            SetPlotNumbersCommand = new DelegateCommand(OnSetPlotNumbersCommand);
            NumberingPrefix = "";
            StartNumber = "1";
            Step = "1";
            //отладка
            SelectedDrawingType = "Сборочные чертежи";
            //отладка
        }

        private void OnSetPlotNumbersCommand()
        {
            //Create a model instance and check connection status
            Model model = new Model();
            if (!model.GetConnectionStatus())
            {
                MessageBox.Show("Tekla Structures not connected");
                return;
            }

            DrawingHandler drawingHandler = new DrawingHandler();
            DrawingEnumerator drawingEnumerator = drawingHandler.GetDrawings();
            List<Drawing> selectedDrawings = SelectionUtils.GetDrawingsList(SelectedDrawingType, drawingEnumerator);

            DrawingComparer drawingComparer = new DrawingComparer();
            selectedDrawings.Sort(drawingComparer);

            int i = Convert.ToInt32(StartNumber);
            foreach (Drawing drawing in selectedDrawings)
            {
                string number = Convert.ToString(i);
                drawing.SetUserProperty("ru_list", number);
                i+= Convert.ToInt32(Step);
                drawing.CommitChanges();
            }

            String message = "Нумерация " + SelectedDrawingType;
            model.CommitChanges(message);
        }

        public event EventHandler CloseRequest;

        private void RaiseCloseRequest()
        {
            CloseRequest?.Invoke(this, EventArgs.Empty);
        }
    }

    class DrawingComparer : IComparer<Drawing>
    {
        public int Compare(Drawing d1, Drawing d2)
        {
            return String.Compare(d1.Mark, d2.Mark);
        }
    }

}
