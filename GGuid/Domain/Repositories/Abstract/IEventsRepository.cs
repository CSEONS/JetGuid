using GGuid.Domain.Entities;

namespace GGuid.Domain.Repositories.Abstract
{
    public interface IEventsRepository
    {
        Task AddAsync(Event item);
        Task<Event> GetByIdAsync(Guid id);
        IQueryable<Event> GetAll();
        Task SaveChangesAsync();
    }
}
