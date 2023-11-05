using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Eventing.Reader;
using System.Text.Json;

namespace GGuid.Domain.Entities
{
    public class Event
    {
        public Guid Id { get; set; }  
        public Guid OwnerId { get; set; }
        public double Latude { get; set; }
        public double Longitude { get; set; }
        public string PreviewImgPath { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public string Format { get; set; }
        public string? SkillsJSON { get; set; }
        public int Duration { get; set; }
        [NotMapped]
        public IEnumerable<string>? Skills
        {
            get
            {
                if (string.IsNullOrEmpty(SkillsJSON))
                    return Enumerable.Empty<string>();

                return JsonSerializer.Deserialize<IEnumerable<string>>(SkillsJSON);
            }
            set
            {
                if (value is null)
                    SkillsJSON = string.Empty;
                else
                    SkillsJSON = JsonSerializer.Serialize(value);
            }
        }
        public DateTime StartTime { get; set; }
        public EventStatus Status { get; set; }
    }

    public enum EventStatus
    {
        Soon,
        Starting,
        End,
        Canceled
    }
}
