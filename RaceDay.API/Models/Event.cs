using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceDay.API.Models
{
    public class Event
    {
        [Key]
        public int EventID { get; set; }

        [Required]
        public int OrganiserID { get; set; }

        [Required]
        [MaxLength(150)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public DateTime EventDate { get; set; }

        [Required]
        [MaxLength(150)]
        public string Location { get; set; } = string.Empty;

        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey(nameof(OrganiserID))]
        public User? Organiser { get; set; }

        public ICollection<Category> Categories { get; set; } = new List<Category>();

        public RouteWeatherInfo? WeatherInfo { get; set; }
    }
}
