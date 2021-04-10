using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace DiagramDesigner
{
    public class ConnectionNode : Control, INotifyPropertyChanged
    {
        // drag start point, relative to the DiagramCanvas
        private Point? dragStartPoint = null;

        public ConnectionNodeOrientation Orientation { get; set; }

        // center position of this ConnectionNode relative to the DiagramCanvas
        private Point position;
        public Point Position
        {
            get { return position; }
            set
            {
                if (position != value)
                {
                    position = value;
                    OnPropertyChanged("Position");
                }
            }
        }

        // the DiagramElement this ConnectionNode belongs to;
        // retrieved from DataContext, which is set in the
        // DiagramElement template
        private DiagramElement parentDiagramElement;
        public DiagramElement ParentDiagramElement
        {
            get
            {
                if (parentDiagramElement == null)
                    parentDiagramElement = this.DataContext as DiagramElement;

                return parentDiagramElement;
            }
        }

        // keep track of connections that link to this ConnectionNode
        private List<Connection> connections;
        public List<Connection> Connections
        {
            get
            {
                if (connections == null)
                    connections = new List<Connection>();
                return connections;
            }
        }

        public ConnectionNode()
        {
            // fired when layout changes
            base.LayoutUpdated += new EventHandler(ConnectionNode_LayoutUpdated);            
        }

        // when the layout changes we update the position property
        void ConnectionNode_LayoutUpdated(object sender, EventArgs e)
        {
            DiagramCanvas designer = GetDiagramCanvas(this);
            if (designer != null)
            {
                //get centre position of this ConnectionNode relative to the DiagramCanvas
                this.Position = this.TransformToAncestor(designer).Transform(new Point(this.Width / 2, this.Height / 2));
            }
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            DiagramCanvas canvas = GetDiagramCanvas(this);
            if (canvas != null)
            {
                // position relative to DiagramCanvas
                this.dragStartPoint = new Point?(e.GetPosition(canvas));
                e.Handled = true;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            // if mouse button is not pressed we have no drag operation, ...
            if (e.LeftButton != MouseButtonState.Pressed)
                this.dragStartPoint = null;

            // but if mouse button is pressed and start point value is set we do have one
            if (this.dragStartPoint.HasValue)
            {
                // create connection adorner 
                DiagramCanvas canvas = GetDiagramCanvas(this);
                if (canvas != null)
                {
                    AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(canvas);
                    if (adornerLayer != null)
                    {
                        ConnectionNodeAdorner adorner = new ConnectionNodeAdorner(canvas, this);
                        if (adorner != null)
                        {
                            adornerLayer.Add(adorner);
                            e.Handled = true;
                        }
                    }
                }
            }
        }

        internal ConnectionNodeInfo GetInfo()
        {
            ConnectionNodeInfo info = new ConnectionNodeInfo();
            info.DiagramElementLeft = DiagramCanvas.GetLeft(this.ParentDiagramElement);
            info.DiagramElementTop = DiagramCanvas.GetTop(this.ParentDiagramElement);
            info.DiagramElementSize = new Size(this.ParentDiagramElement.ActualWidth, this.ParentDiagramElement.ActualHeight);
            info.Orientation = this.Orientation;
            info.Position = this.Position;
            return info;
        }

        // iterate through visual tree to get parent DiagramCanvas
        private DiagramCanvas GetDiagramCanvas(DependencyObject element)
        {
            while (element != null && !(element is DiagramCanvas))
                element = VisualTreeHelper.GetParent(element);

            return element as DiagramCanvas;
        }

        #region INotifyPropertyChanged Members

        // we could use DependencyProperties as well to inform others of property changes
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        #endregion
    }

    // provides compact info about a ConnectionNode; used for the 
    // routing algorithm, instead of hand over a full fledged ConnectionNode
    internal struct ConnectionNodeInfo
    {
        public double DiagramElementLeft { get; set; }
        public double DiagramElementTop { get; set; }
        public Size DiagramElementSize { get; set; }
        public Point Position { get; set; }
        public ConnectionNodeOrientation Orientation { get; set; }
    }

    public enum ConnectionNodeOrientation
    {
        None,
        Left,
        Top,
        Right,
        Bottom
    }
}
