using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace DiagramDesigner.Controls
{
    public class ResizeControl : Thumb
    {
        public ResizeControl()
        {
            base.DragDelta += new DragDeltaEventHandler(ResizeControl_DragDelta);
        }

        void ResizeControl_DragDelta(object sender, DragDeltaEventArgs e)
        {
            DiagramElement DiagramElement = this.DataContext as DiagramElement;
            DiagramCanvas designer = VisualTreeHelper.GetParent(DiagramElement) as DiagramCanvas;

            if (DiagramElement != null && designer != null && DiagramElement.IsSelected)
            {
                double minLeft, minTop, minDeltaHorizontal, minDeltaVertical;
                double dragDeltaVertical, dragDeltaHorizontal, scale;

                IEnumerable<DiagramElement> selectedDiagramElements = designer.SelectionService.CurrentSelection.OfType<DiagramElement>();

                CalculateDragLimits(selectedDiagramElements, out minLeft, out minTop,
                                    out minDeltaHorizontal, out minDeltaVertical);

                foreach (DiagramElement item in selectedDiagramElements)
                {
                    if (item != null && item.ParentID == Guid.Empty)
                    {
                        switch (base.VerticalAlignment)
                        {
                            case VerticalAlignment.Bottom:
                                dragDeltaVertical = Math.Min(-e.VerticalChange, minDeltaVertical);
                                scale = (item.ActualHeight - dragDeltaVertical) / item.ActualHeight;
                                DragBottom(scale, item, designer.SelectionService);
                                break;
                            case VerticalAlignment.Top:
                                double top = Canvas.GetTop(item);
                                dragDeltaVertical = Math.Min(Math.Max(-minTop, e.VerticalChange), minDeltaVertical);
                                scale = (item.ActualHeight - dragDeltaVertical) / item.ActualHeight;
                                DragTop(scale, item, designer.SelectionService);
                                break;
                            default:
                                break;
                        }

                        switch (base.HorizontalAlignment)
                        {
                            case HorizontalAlignment.Left:
                                double left = Canvas.GetLeft(item);
                                dragDeltaHorizontal = Math.Min(Math.Max(-minLeft, e.HorizontalChange), minDeltaHorizontal);
                                scale = (item.ActualWidth - dragDeltaHorizontal) / item.ActualWidth;
                                DragLeft(scale, item, designer.SelectionService);
                                break;
                            case HorizontalAlignment.Right:
                                dragDeltaHorizontal = Math.Min(-e.HorizontalChange, minDeltaHorizontal);
                                scale = (item.ActualWidth - dragDeltaHorizontal) / item.ActualWidth;
                                DragRight(scale, item, designer.SelectionService);
                                break;
                            default:
                                break;
                        }
                    }
                }
                e.Handled = true;
            }
        }

        #region Helper methods

        private void DragLeft(double scale, DiagramElement item, Selection selectionService)
        {
            IEnumerable<DiagramElement> groupItems = selectionService.GetGroupMembers(item).Cast<DiagramElement>();

            double groupLeft = Canvas.GetLeft(item) + item.Width;
            foreach (DiagramElement groupItem in groupItems)
            {
                double groupItemLeft = Canvas.GetLeft(groupItem);
                double delta = (groupLeft - groupItemLeft) * (scale - 1);
                Canvas.SetLeft(groupItem, groupItemLeft - delta);
                groupItem.Width = groupItem.ActualWidth * scale;
            }
        }

        private void DragTop(double scale, DiagramElement item, Selection selectionService)
        {
            IEnumerable<DiagramElement> groupItems = selectionService.GetGroupMembers(item).Cast<DiagramElement>();
            double groupBottom = Canvas.GetTop(item) + item.Height;
            foreach (DiagramElement groupItem in groupItems)
            {
                double groupItemTop = Canvas.GetTop(groupItem);
                double delta = (groupBottom - groupItemTop) * (scale - 1);
                Canvas.SetTop(groupItem, groupItemTop - delta);
                groupItem.Height = groupItem.ActualHeight * scale;
            }
        }

        private void DragRight(double scale, DiagramElement item, Selection selectionService)
        {
            IEnumerable<DiagramElement> groupItems = selectionService.GetGroupMembers(item).Cast<DiagramElement>();

            double groupLeft = Canvas.GetLeft(item);
            foreach (DiagramElement groupItem in groupItems)
            {
                double groupItemLeft = Canvas.GetLeft(groupItem);
                double delta = (groupItemLeft - groupLeft) * (scale - 1);

                Canvas.SetLeft(groupItem, groupItemLeft + delta);
                groupItem.Width = groupItem.ActualWidth * scale;
            }
        }

        private void DragBottom(double scale, DiagramElement item, Selection selectionService)
        {
            IEnumerable<DiagramElement> groupItems = selectionService.GetGroupMembers(item).Cast<DiagramElement>();
            double groupTop = Canvas.GetTop(item);
            foreach (DiagramElement groupItem in groupItems)
            {
                double groupItemTop = Canvas.GetTop(groupItem);
                double delta = (groupItemTop - groupTop) * (scale - 1);

                Canvas.SetTop(groupItem, groupItemTop + delta);
                groupItem.Height = groupItem.ActualHeight * scale;
            }
        }

        private void CalculateDragLimits(IEnumerable<DiagramElement> selectedItems, out double minLeft, out double minTop, out double minDeltaHorizontal, out double minDeltaVertical)
        {
            minLeft = double.MaxValue;
            minTop = double.MaxValue;
            minDeltaHorizontal = double.MaxValue;
            minDeltaVertical = double.MaxValue;

            // drag limits are set by these parameters: canvas top, canvas left, minHeight, minWidth
            // calculate min value for each parameter for each item
            foreach (DiagramElement item in selectedItems)
            {
                double left = Canvas.GetLeft(item);
                double top = Canvas.GetTop(item);

                minLeft = double.IsNaN(left) ? 0 : Math.Min(left, minLeft);
                minTop = double.IsNaN(top) ? 0 : Math.Min(top, minTop);

                minDeltaVertical = Math.Min(minDeltaVertical, item.ActualHeight - item.MinHeight);
                minDeltaHorizontal = Math.Min(minDeltaHorizontal, item.ActualWidth - item.MinWidth);
            }
        }

        #endregion
    }
}
