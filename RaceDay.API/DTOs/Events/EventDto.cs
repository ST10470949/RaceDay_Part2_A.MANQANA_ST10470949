namespace RaceDay.API.DTOs.Events
{
    public class EventDto
    {
        public int EventID { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime EventDate { get; set; }
        public string Location { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int OrganiserID { get; set; }
        public string? OrganiserName { get; set; }
    }
}
