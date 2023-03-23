using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekla.Structures.Drawing;

namespace PlotNumbering
{
    public class SelectionUtils
    {
        public static List<Drawing> GetDrawingsList(String drawingType)
        {
            DrawingHandler drawingHandler = new DrawingHandler();
            DrawingEnumerator drawingEnumerator = drawingHandler.GetDrawings();
            List<Drawing> drawingsList = new List<Drawing>();

            if (drawingType == "Сборочные чертежи")
            {
                foreach (Drawing drawing in drawingEnumerator)
                {
                    if (drawing is GADrawing)
                        drawingsList.Add(drawing);
                }
            }
            else if (drawingType == "Отправочные марки")
            {
                foreach (Drawing drawing in drawingEnumerator)
                {
                    if (drawing is AssemblyDrawing)
                        drawingsList.Add(drawing);
                }
            }
            else if (drawingType == "Отдельные детали")
            {
                foreach (Drawing drawing in drawingEnumerator)
                {
                    if (drawing is SinglePartDrawing)
                        drawingsList.Add(drawing);
                }
            }

            return drawingsList;
        }
    }
}
