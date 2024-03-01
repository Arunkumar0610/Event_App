using EventService.Models;

namespace EventService.Services
{
    public interface IEventService
    {
        public Task<EventDataResponse> GetEventsAsync();
        public Task<EventItem> GetEventItemById(int id);

    }
}
