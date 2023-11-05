using GGuid.Domain.Entities;
using System.Diagnostics.Eventing.Reader;

namespace GGuid.Models
{
    public class EventViewModel
    {
        public Guid Id { get; set; }
        public string PreviewImgPath { get; set; }
        public double Latude { get; set; }
        public double Longitude { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public string Format { get; set; }
        public IEnumerable<string> Skills { get; set; }
        public DateTime StartTime { get; set; }
        public EventStatus Status { get; set; }
    }
}
