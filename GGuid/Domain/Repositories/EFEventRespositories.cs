using GGuid.Domain.Entities;
using GGuid.Domain.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace GGuid.Domain.Repositories
{
    public class EFEventRespositories : IEventsRepository
    {
        private readonly AppDbContext _context;

        public EFEventRespositories(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Event item)
        {
            if (item.Id == new Guid())
                await _context.AddAsync(item);
            else
                _context.Update(item);

            _context.SaveChanges();
        }

        public IQueryable<Event> GetAll()
        {
            return _context.Events;
        }

        public async Task<Event> GetByIdAsync(Guid id)
        {
            return await _context.Events.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
