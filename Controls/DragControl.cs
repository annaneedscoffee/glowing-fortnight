using System;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace DiagramDesigner.Controls
{
    public class DragControl : Thumb
    {
        public DragControl()
        {
            base.DragDelta += new DragDeltaEventHandler(DragControl_DragDelta);
        }

        void DragControl_DragDelta(object sender, DragDeltaEventArgs e)
        {
            DiagramElement DiagramElement = this.DataContext as DiagramElement;
            DiagramCanvas designer = VisualTreeHelper.GetParent(DiagramElement) as DiagramCanvas;
            if (DiagramElement != null && designer != null && DiagramElement.IsSelected)
            {
                double minLeft = double.MaxValue;
                double minTop = double.MaxValue;

                // we only move DiagramElements
                var DiagramElements = designer.SelectionService.CurrentSelection.OfType<DiagramElement>();

                foreach (DiagramElement item in DiagramElements)
                {
                    double left = Canvas.GetLeft(item);
                    double top = Canvas.GetTop(item);

                    minLeft = double.IsNaN(left) ? 0 : Math.Min(left, minLeft);
                    minTop = double.IsNaN(top) ? 0 : Math.Min(top, minTop);
                }

                double deltaHorizontal = Math.Max(-minLeft, e.HorizontalChange);
                double deltaVertical = Math.Max(-minTop, e.VerticalChange);

                foreach (DiagramElement item in DiagramElements)
                {
                    double left = Canvas.GetLeft(item);
                    double top = Canvas.GetTop(item);

                    if (double.IsNaN(left)) left = 0;
                    if (double.IsNaN(top)) top = 0;

                    Canvas.SetLeft(item, left + deltaHorizontal);
                    Canvas.SetTop(item, top + deltaVertical);
                }

                designer.InvalidateMeasure();
                e.Handled = true;
            }
        }
    }
}
