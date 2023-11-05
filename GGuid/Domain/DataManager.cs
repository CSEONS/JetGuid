using GGuid.Domain.Repositories.Abstract;

namespace GGuid.Domain
{
    public class DataManager
    {
        public readonly IEventsRepository Events;

        public DataManager(IEventsRepository eventsRepository)
        {
            Events = eventsRepository;
        }
    }
}
