using Microsoft.AspNetCore.Identity;

namespace GGuid.Domain.Entities
{
    public class AppUser : IdentityUser<Guid>
    {
        public string? ProfileImagePath { get; set; }
        public virtual ICollection<Event> Events { get; set; }
        public virtual ICollection<Event> JoinEvents { get; set; }
        public int VisitCount { get; set; }
        public int CreateEventCount { get; set; }
    }
}
