using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace DiagramDesigner
{
    public class SelectionAdorner : Adorner
    {
        private Point? startPoint;
        private Point? endPoint;
        private Pen SelectionPen;

        private DiagramCanvas DiagramCanvas;

        public SelectionAdorner(DiagramCanvas DiagramCanvas, Point? dragStartPoint)
            : base(DiagramCanvas)
        {
            this.DiagramCanvas = DiagramCanvas;
            this.startPoint = dragStartPoint;
            SelectionPen = new Pen(Brushes.LightSlateGray, 1);
            SelectionPen.DashStyle = new DashStyle(new double[] { 2 }, 1);
        }

        protected override void OnMouseMove(System.Windows.Input.MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (!this.IsMouseCaptured)
                    this.CaptureMouse();

                endPoint = e.GetPosition(this);
                UpdateSelection();
                this.InvalidateVisual();
            }
            else
            {
                if (this.IsMouseCaptured) this.ReleaseMouseCapture();
            }

            e.Handled = true;
        }

        protected override void OnMouseUp(System.Windows.Input.MouseButtonEventArgs e)
        {
            // release mouse capture
            if (this.IsMouseCaptured) this.ReleaseMouseCapture();

            // remove this adorner from adorner layer
            AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(this.DiagramCanvas);
            if (adornerLayer != null)
                adornerLayer.Remove(this);

            e.Handled = true;
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);

            // without a background the OnMouseMove event would not be fired!
            // Alternative: implement a Canvas as a child of this adorner, like
            // the ConnectionAdorner does.
            dc.DrawRectangle(Brushes.Transparent, null, new Rect(RenderSize));

            if (this.startPoint.HasValue && this.endPoint.HasValue)
                dc.DrawRectangle(Brushes.Transparent, SelectionPen, new Rect(this.startPoint.Value, this.endPoint.Value));
        }

        private void UpdateSelection()
        {
            DiagramCanvas.SelectionService.ClearSelection();

            Rect Selection = new Rect(startPoint.Value, endPoint.Value);
            foreach (Control item in DiagramCanvas.Children)
            {
                Rect itemRect = VisualTreeHelper.GetDescendantBounds(item);
                Rect itemBounds = item.TransformToAncestor(DiagramCanvas).TransformBounds(itemRect);

                if (Selection.Contains(itemBounds))
                {
                    if (item is Connection)
                        DiagramCanvas.SelectionService.AddToSelection(item as ISelectable);
                    else
                    {
                        DiagramElement di = item as DiagramElement;
                        if (di.ParentID == Guid.Empty)
                            DiagramCanvas.SelectionService.AddToSelection(di);
                    }
                }
            }
        }
    }
}
