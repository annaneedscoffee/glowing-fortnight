using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace DiagramDesigner
{
    public class ConnectionAdorner : Adorner
    {
        private DiagramCanvas DiagramCanvas;
        private Canvas adornerCanvas;
        private Connection connection;
        private PathGeometry pathGeometry;
        private ConnectionNode fixConnectionNode, dragConnectionNode;
        private Thumb sourceDragControl, targetDragControl;
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

        private VisualCollection visualChildren;
        protected override int VisualChildrenCount
        {
            get
            {
                return this.visualChildren.Count;
            }
        }

        protected override Visual GetVisualChild(int index)
        {
            return this.visualChildren[index];
        }

        public ConnectionAdorner(DiagramCanvas designer, Connection connection)
            : base(designer)
        {
            this.DiagramCanvas = designer;
            adornerCanvas = new Canvas();
            this.visualChildren = new VisualCollection(this);
            this.visualChildren.Add(adornerCanvas);

            this.connection = connection;
            this.connection.PropertyChanged += new PropertyChangedEventHandler(AnchorPositionChanged);

            InitializeDragControls();

            drawingPen = new Pen(Brushes.LightSlateGray, 1);
            drawingPen.LineJoin = PenLineJoin.Round;

            base.Unloaded += new RoutedEventHandler(ConnectionAdorner_Unloaded);
        }
                

        void AnchorPositionChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("AnchorPositionSource"))
            {
                Canvas.SetLeft(sourceDragControl, connection.AnchorPositionSource.X);
                Canvas.SetTop(sourceDragControl, connection.AnchorPositionSource.Y);
            }

            if (e.PropertyName.Equals("AnchorPositionTarget"))
            {
                Canvas.SetLeft(targetDragControl, connection.AnchorPositionTarget.X);
                Canvas.SetTop(targetDragControl, connection.AnchorPositionTarget.Y);
            }
        }

        void thumbDragControl_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            if (HitConnectionNode != null)
            {
                if (connection != null)
                {
                    if (connection.Source == fixConnectionNode)
                        connection.Target = this.HitConnectionNode;
                    else
                        connection.Source = this.HitConnectionNode;
                }
            }

            this.HitDiagramElement = null;
            this.HitConnectionNode = null;
            this.pathGeometry = null;
            this.connection.StrokeDashArray = null;
            this.InvalidateVisual();
        }

        void thumbDragControl_DragStarted(object sender, DragStartedEventArgs e)
        {
            this.HitDiagramElement = null;
            this.HitConnectionNode = null;
            this.pathGeometry = null;
            this.Cursor = Cursors.Cross;
            this.connection.StrokeDashArray = new DoubleCollection(new double[] { 1, 2 });

            if (sender == sourceDragControl)
            {
                fixConnectionNode = connection.Target;
                dragConnectionNode = connection.Source;
            }
            else if (sender == targetDragControl)
            {
                dragConnectionNode = connection.Target;
                fixConnectionNode = connection.Source;
            }
        }

        void thumbDragControl_DragDelta(object sender, DragDeltaEventArgs e)
        {
            Point currentPosition = Mouse.GetPosition(this);
            this.HitTesting(currentPosition);
            this.pathGeometry = UpdatePathGeometry(currentPosition);
            this.InvalidateVisual();
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
            dc.DrawGeometry(null, drawingPen, this.pathGeometry);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            adornerCanvas.Arrange(new Rect(0, 0, this.DiagramCanvas.ActualWidth, this.DiagramCanvas.ActualHeight));
            return finalSize;
        }

        private void ConnectionAdorner_Unloaded(object sender, RoutedEventArgs e)
        {
            sourceDragControl.DragDelta -= new DragDeltaEventHandler(thumbDragControl_DragDelta);
            sourceDragControl.DragStarted -= new DragStartedEventHandler(thumbDragControl_DragStarted);
            sourceDragControl.DragCompleted -= new DragCompletedEventHandler(thumbDragControl_DragCompleted);

            targetDragControl.DragDelta -= new DragDeltaEventHandler(thumbDragControl_DragDelta);
            targetDragControl.DragStarted -= new DragStartedEventHandler(thumbDragControl_DragStarted);
            targetDragControl.DragCompleted -= new DragCompletedEventHandler(thumbDragControl_DragCompleted);
        }

        private void InitializeDragControls()
        {
            Style DragControlStyle = connection.FindResource("ConnectionAdornerThumbStyle") as Style;

            //source drag thumb
            sourceDragControl = new Thumb();
            Canvas.SetLeft(sourceDragControl, connection.AnchorPositionSource.X);
            Canvas.SetTop(sourceDragControl, connection.AnchorPositionSource.Y);
            this.adornerCanvas.Children.Add(sourceDragControl);
            if (DragControlStyle != null)
                sourceDragControl.Style = DragControlStyle;

            sourceDragControl.DragDelta += new DragDeltaEventHandler(thumbDragControl_DragDelta);
            sourceDragControl.DragStarted += new DragStartedEventHandler(thumbDragControl_DragStarted);
            sourceDragControl.DragCompleted += new DragCompletedEventHandler(thumbDragControl_DragCompleted);

            // target drag thumb
            targetDragControl = new Thumb();
            Canvas.SetLeft(targetDragControl, connection.AnchorPositionTarget.X);
            Canvas.SetTop(targetDragControl, connection.AnchorPositionTarget.Y);
            this.adornerCanvas.Children.Add(targetDragControl);
            if (DragControlStyle != null)
                targetDragControl.Style = DragControlStyle;

            targetDragControl.DragDelta += new DragDeltaEventHandler(thumbDragControl_DragDelta);
            targetDragControl.DragStarted += new DragStartedEventHandler(thumbDragControl_DragStarted);
            targetDragControl.DragCompleted += new DragCompletedEventHandler(thumbDragControl_DragCompleted);
        }

        private PathGeometry UpdatePathGeometry(Point position)
        {
            PathGeometry geometry = new PathGeometry();

            ConnectionNodeOrientation targetOrientation;
            if (HitConnectionNode != null)
                targetOrientation = HitConnectionNode.Orientation;
            else
                targetOrientation = dragConnectionNode.Orientation;

            List<Point> linePoints = PathFinder.GetConnectionLine(fixConnectionNode.GetInfo(), position, targetOrientation);

            if (linePoints.Count > 0)
            {
                PathFigure figure = new PathFigure();
                figure.StartPoint = linePoints[0];
                linePoints.Remove(linePoints[0]);
                figure.Segments.Add(new PolyLineSegment(linePoints, true));
                geometry.Figures.Add(figure);
            }

            return geometry;
        }

        private void HitTesting(Point hitPoint)
        {
            bool hitConnectionNodeFlag = false;

            DependencyObject hitObject = DiagramCanvas.InputHitTest(hitPoint) as DependencyObject;
            while (hitObject != null &&
                   hitObject != fixConnectionNode.ParentDiagramElement &&
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
