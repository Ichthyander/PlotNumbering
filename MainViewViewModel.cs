using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Windows;
using Tekla.Structures.Model;
using Tekla.Structures.Drawing;
using System.Windows.Controls;

namespace PlotNumbering
{
    public class MainViewViewModel
    {
        public DelegateCommand SetPlotNumbersCommand { get; }

        public String NumberingPrefix { get; set; } //Currently useless. Need an implementation
        public String StartNumber { get; set; }
        public String Step { get; set; }
        public ComboBoxItem SelectedDrawingType { get; set; }

        public MainViewViewModel()
        {
            SetPlotNumbersCommand = new DelegateCommand(OnSetPlotNumbersCommand);
            NumberingPrefix = "";
            StartNumber = "1";
            Step = "1";
            //отладка
            //SelectedDrawingType = "Сборочные чертежи";
            //отладка
        }

        private void OnSetPlotNumbersCommand()
        {
            //Creating a model instance and check connection status
            Model model = new Model();
            if (!model.GetConnectionStatus())
            {
                MessageBox.Show("Tekla Structures not connected");
                return;
            }

            DrawingHandler drawingHandler = new DrawingHandler();
            DrawingEnumerator drawingEnumerator = drawingHandler.GetDrawings();
            //Need to get exception handler on empty input, or exclude possibility of empty object. 
            List<Drawing> selectedDrawings = SelectionUtils.GetDrawingsList((String)SelectedDrawingType.Content, drawingEnumerator);

            DrawingComparer drawingComparer = new DrawingComparer();
            selectedDrawings.Sort(drawingComparer);

            int i = Convert.ToInt32(StartNumber);
            foreach (Drawing drawing in selectedDrawings)
            {
                string number = Convert.ToString(i);
                drawing.SetUserProperty("ru_list", number);
                i += Convert.ToInt32(Step);
                drawing.CommitChanges();
            }

            String message = "Нумерация " + SelectedDrawingType;
            model.CommitChanges(message);

            //MessageBox.Show((String)SelectedDrawingType.Content);
            //return;
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
