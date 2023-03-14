using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Windows;
using Tekla.Structures.Model;
using Tekla.Structures.Drawing;

namespace PlotNumbering
{
    class MainViewViewModel
    {
        public DelegateCommand SetPlotNumbersCommand { get; }

        public MainViewViewModel()
        {
            SetPlotNumbersCommand = new DelegateCommand(OnSetPlotNumbersCommand);
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

            ////Get model info and send a "Hello World" message to the message box
            //ModelInfo modelInfo = model.GetInfo();
            //string name = modelInfo.ModelName;

            //MessageBox.Show(string.Format("Hello! Your current model name is {0}", name));

            ////Send a hello world message to Tekla Structures user command prompt
            //Operation.DisplayPrompt(string.Format("Hello! Your current model name is {0}", name));

            DrawingHandler drawingHandler = new DrawingHandler();
            DrawingEnumerator drawingEnumerator = drawingHandler.GetDrawings();
            List<Drawing> assemblyDrawings = new List<Drawing>();

            foreach (Drawing drawing in drawingEnumerator)
            {
                if (drawing is AssemblyDrawing)
                    assemblyDrawings.Add(drawing);
            }

            DrawingComparer drawingComparer = new DrawingComparer();
            assemblyDrawings.Sort(drawingComparer);

            int i = 101;
            foreach (Drawing assemblyDrawing in assemblyDrawings)
            {
                string number = Convert.ToString(i);
                assemblyDrawing.SetUserProperty("ru_list", number);
                i++;
                assemblyDrawing.CommitChanges();
            }

            //foreach (Drawing assemblyDrawing in assemblyDrawings)
            //{
            //    assemblyDrawing.CommitChanges();
            //}
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
