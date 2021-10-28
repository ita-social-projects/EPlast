using EPlast.BLL.Interfaces.Events;

namespace EPlast.BLL.Services.Events
{
    public class EventWrapper : IEventWrapper
    {
        public IEventCategoryManager EventCategoryManager { get; }

        public IEventTypeManager EventTypeManager { get; }

        public IEventStatusManager EventStatusManager { get; }

        public IEventGalleryManager EventGalleryManager { get; }

        public IEventSectionManager EventSectionManager { get; }

        public EventWrapper(IEventCategoryManager eventCategoryManager, IEventTypeManager eventTypeManager,
            IEventStatusManager eventStatusManager, IEventGalleryManager eventGalleryManager, IEventSectionManager eventSectionManager)
        {
            EventCategoryManager = eventCategoryManager;
            EventTypeManager = eventTypeManager;
            EventStatusManager = eventStatusManager;
            EventGalleryManager = eventGalleryManager;
            EventSectionManager = eventSectionManager;
        }
    }
}
