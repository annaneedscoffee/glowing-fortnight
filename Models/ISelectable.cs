
namespace DiagramDesigner
{
    // Common interface for items that can be selected
    // on the DiagramCanvas; used by DiagramElement and Connection
    public interface ISelectable
    {
        bool IsSelected { get; set; }
    }
}
