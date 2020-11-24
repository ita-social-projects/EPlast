using EPlast.BLL.Interfaces.Events;

namespace EPlast.BLL.Services.Events
{
    public class EventWrapper : IEventWrapper
    {
        public IEventCategoryManager EventCategoryManager { get; }

        public IEventTypeManager EventTypeManager { get; }

        public IEventStatusManager EventStatusManager { get; }

        public IEventGalleryManager EventGalleryManager { get; }

        public EventWrapper(IEventCategoryManager eventCategoryManager, IEventTypeManager eventTypeManager,
            IEventStatusManager eventStatusManager, IEventGalleryManager eventGalleryManager)
        {
            EventCategoryManager = eventCategoryManager;
            EventTypeManager = eventTypeManager;
            EventStatusManager = eventStatusManager;
            EventGalleryManager = eventGalleryManager;
        }
    }
}
