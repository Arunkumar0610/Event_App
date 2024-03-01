namespace EventService.Models
{
    public class EventItem
    {
        public int Id { get; set; }
        public string? Type { get; set; }
        public string? Title { get; set; }
        public string? Short_title { get; set; }
        public DateTime? Datetime_utc { get; set; }
        public DateTime? Visible_at { get; set; }
        public DateTime? Visible_until { get; set; }
        public Venue? Venue { get; set; }
        public List<Performer>? Performers { get; set; }

    }
}
