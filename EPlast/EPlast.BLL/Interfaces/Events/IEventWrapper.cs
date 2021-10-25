
namespace EPlast.BLL.Interfaces.Events
{
    public interface IEventWrapper
    {
        IEventCategoryManager EventCategoryManager { get; }
        IEventTypeManager EventTypeManager { get; }
        IEventStatusManager EventStatusManager { get; }
        IEventGalleryManager EventGalleryManager { get; }
        IEventSectionManager EventSectionManager { get; }
    }
}
