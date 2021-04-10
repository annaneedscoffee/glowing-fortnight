using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using DiagramDesigner.Controls;

namespace DiagramDesigner
{
    //These attributes identify the types of the named parts that are used for templating
    [TemplatePart(Name = "DragControl", Type = typeof(DragControl))]
    [TemplatePart(Name = "ResizeDecorator", Type = typeof(Control))]
    [TemplatePart(Name = "ConnectionNodeDecorator", Type = typeof(Control))]
    [TemplatePart(Name = "ContentPresenter", Type = typeof(ContentPresenter))]
    public class DiagramElement : ContentControl, ISelectable, IGroupable
    {
        #region ID
        private Guid id;
        public Guid ID
        {
            get { return id; }
        }
        #endregion

        #region ParentID
        public Guid ParentID
        {
            get { return (Guid)GetValue(ParentIDProperty); }
            set { SetValue(ParentIDProperty, value); }
        }
        public static readonly DependencyProperty ParentIDProperty = DependencyProperty.Register("ParentID", typeof(Guid), typeof(DiagramElement));
        #endregion

        #region IsGroup
        public bool IsGroup
        {
            get { return (bool)GetValue(IsGroupProperty); }
            set { SetValue(IsGroupProperty, value); }
        }
        public static readonly DependencyProperty IsGroupProperty =
            DependencyProperty.Register("IsGroup", typeof(bool), typeof(DiagramElement));
        #endregion

        #region IsSelected

        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }
        public static readonly DependencyProperty IsSelectedProperty =
          DependencyProperty.Register("IsSelected",
                                       typeof(bool),
                                       typeof(DiagramElement),
                                       new FrameworkPropertyMetadata(false));

        #endregion

        #region DragControlTemplate Property

        // can be used to replace the default template for the DragControl
        public static readonly DependencyProperty DragControlTemplateProperty =
            DependencyProperty.RegisterAttached("DragControlTemplate", typeof(ControlTemplate), typeof(DiagramElement));

        public static ControlTemplate GetDragControlTemplate(UIElement element)
        {
            return (ControlTemplate)element.GetValue(DragControlTemplateProperty);
        }

        public static void SetDragControlTemplate(UIElement element, ControlTemplate value)
        {
            element.SetValue(DragControlTemplateProperty, value);
        }

        #endregion

        #region ConnectionNodeDecoratorTemplate Property

        // can be used to replace the default template for the ConnectionNodeDecorator
        public static readonly DependencyProperty ConnectionNodeDecoratorTemplateProperty =
            DependencyProperty.RegisterAttached("ConnectionNodeDecoratorTemplate", typeof(ControlTemplate), typeof(DiagramElement));

        public static ControlTemplate GetConnectionNodeDecoratorTemplate(UIElement element)
        {
            return (ControlTemplate)element.GetValue(ConnectionNodeDecoratorTemplateProperty);
        }

        public static void SetConnectionNodeDecoratorTemplate(UIElement element, ControlTemplate value)
        {
            element.SetValue(ConnectionNodeDecoratorTemplateProperty, value);
        }

        #endregion

        #region IsDragConnectionOver

        // while drag connection procedure is ongoing and the mouse moves over 
        // this item this value is true; if true the ConnectionNodeDecorator is triggered
        // to be visible, see template
        public bool IsDragConnectionOver
        {
            get { return (bool)GetValue(IsDragConnectionOverProperty); }
            set { SetValue(IsDragConnectionOverProperty, value); }
        }
        public static readonly DependencyProperty IsDragConnectionOverProperty =
            DependencyProperty.Register("IsDragConnectionOver",
                                         typeof(bool),
                                         typeof(DiagramElement),
                                         new FrameworkPropertyMetadata(false));

        #endregion

        static DiagramElement()
        {
            // set the key to reference the style for this control
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(
                typeof(DiagramElement), new FrameworkPropertyMetadata(typeof(DiagramElement)));
        }

        public DiagramElement(Guid id)
        {
            this.id = id;
            this.Loaded += new RoutedEventHandler(DiagramElement_Loaded);
        }

        public DiagramElement()
            : this(Guid.NewGuid())
        {
        }


        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseDown(e);
            DiagramCanvas designer = VisualTreeHelper.GetParent(this) as DiagramCanvas;

            // update selection
            if (designer != null)
            {
                if ((Keyboard.Modifiers & (ModifierKeys.Shift | ModifierKeys.Control)) != ModifierKeys.None)
                    if (this.IsSelected)
                    {
                        designer.SelectionService.RemoveFromSelection(this);
                    }
                    else
                    {
                        designer.SelectionService.AddToSelection(this);
                    }
                else if (!this.IsSelected)
                {
                    designer.SelectionService.SelectItem(this);
                }
                Focus();
            }

            e.Handled = false;
        }

        void DiagramElement_Loaded(object sender, RoutedEventArgs e)
        {
            if (base.Template != null)
            {
                ContentPresenter contentPresenter =
                    this.Template.FindName("ContentPresenter", this) as ContentPresenter;
                if (contentPresenter != null)
                {
                    UIElement contentVisual = VisualTreeHelper.GetChild(contentPresenter, 0) as UIElement;
                    if (contentVisual != null)
                    {
                        DragControl thumb = this.Template.FindName("DragControl", this) as DragControl;
                        if (thumb != null)
                        {
                            ControlTemplate template =
                                DiagramElement.GetDragControlTemplate(contentVisual) as ControlTemplate;
                            if (template != null)
                                thumb.Template = template;
                        }
                    }
                }
            }
        }
    }
}
