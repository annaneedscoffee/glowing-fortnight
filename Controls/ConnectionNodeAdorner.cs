using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls;

namespace DiagramDesigner
{
    public class ConnectionNodeAdorner : Adorner
    {
        private PathGeometry pathGeometry;
        private DiagramCanvas DiagramCanvas;
        private ConnectionNode sourceConnectionNode;
        private Pen drawingPen;

        private DiagramElement hitDiagramElement;
        private DiagramElement HitDiagramElement
        {
            get { return hitDiagramElement; }
            set
            {
                if (hitDiagramElement != value)
                {
                    if (hitDiagramElement != null)
                        hitDiagramElement.IsDragConnectionOver = false;

                    hitDiagramElement = value;

                    if (hitDiagramElement != null)
                        hitDiagramElement.IsDragConnectionOver = true;
                }
            }
        }

        private ConnectionNode hitConnectionNode;
        private ConnectionNode HitConnectionNode
        {
            get { return hitConnectionNode; }
            set
            {
                if (hitConnectionNode != value)
                {
                    hitConnectionNode = value;
                }
            }
        }

        public ConnectionNodeAdorner(DiagramCanvas designer, ConnectionNode sourceConnectionNode)
            : base(designer)
        {
            this.DiagramCanvas = designer;
            this.sourceConnectionNode = sourceConnectionNode;
            drawingPen = new Pen(Brushes.LightSlateGray, 1);
            drawingPen.LineJoin = PenLineJoin.Round;
            this.Cursor = Cursors.Cross;
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            if (HitConnectionNode != null)
            {
                ConnectionNode sourceConnectionNode = this.sourceConnectionNode;
                ConnectionNode targetConnectionNode = this.HitConnectionNode;
                Connection newConnection = new Connection(sourceConnectionNode, targetConnectionNode);

                Canvas.SetZIndex(newConnection, DiagramCanvas.Children.Count);
                this.DiagramCanvas.Children.Add(newConnection);
                
            }
            if (HitDiagramElement != null)
            {
                this.HitDiagramElement.IsDragConnectionOver = false;
            }

            if (this.IsMouseCaptured) this.ReleaseMouseCapture();

            AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(this.DiagramCanvas);
            if (adornerLayer != null)
            {
                adornerLayer.Remove(this);
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (!this.IsMouseCaptured) this.CaptureMouse();
                HitTesting(e.GetPosition(this));
                this.pathGeometry = GetPathGeometry(e.GetPosition(this));
                this.InvalidateVisual();
            }
            else
            {
                if (this.IsMouseCaptured) this.ReleaseMouseCapture();
            }
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
            dc.DrawGeometry(null, drawingPen, this.pathGeometry);

            // without a background the OnMouseMove event would not be fired
            // Alternative: implement a Canvas as a child of this adorner, like
            // the ConnectionAdorner does.
            dc.DrawRectangle(Brushes.Transparent, null, new Rect(RenderSize));
        }

        private PathGeometry GetPathGeometry(Point position)
        {
            PathGeometry geometry = new PathGeometry();

            ConnectionNodeOrientation targetOrientation;
            if (HitConnectionNode != null)
                targetOrientation = HitConnectionNode.Orientation;
            else
                targetOrientation = ConnectionNodeOrientation.None;

            List<Point> pathPoints = PathFinder.GetConnectionLine(sourceConnectionNode.GetInfo(), position, targetOrientation);

            if (pathPoints.Count > 0)
            {
                PathFigure figure = new PathFigure();
                figure.StartPoint = pathPoints[0];
                pathPoints.Remove(pathPoints[0]);
                figure.Segments.Add(new PolyLineSegment(pathPoints, true));
                geometry.Figures.Add(figure);
            }

            return geometry;
        }

        private void HitTesting(Point hitPoint)
        {
            bool hitConnectionNodeFlag = false;

            DependencyObject hitObject = DiagramCanvas.InputHitTest(hitPoint) as DependencyObject;
            while (hitObject != null &&
                   hitObject != sourceConnectionNode.ParentDiagramElement &&
                   hitObject.GetType() != typeof(DiagramCanvas))
            {
                if (hitObject is ConnectionNode)
                {
                    HitConnectionNode = hitObject as ConnectionNode;
                    hitConnectionNodeFlag = true;
                }

                if (hitObject is DiagramElement)
                {
                    HitDiagramElement = hitObject as DiagramElement;
                    if (!hitConnectionNodeFlag)
                        HitConnectionNode = null;
                    return;
                }
                hitObject = VisualTreeHelper.GetParent(hitObject);
            }

            HitConnectionNode = null;
            HitDiagramElement = null;
        }
    }
}
