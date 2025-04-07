namespace EventEaseMJDST10356144.Models
{
    public class Event
    {
        public int Id { get; set; }
        public int VenueId { get; set; }
        public Venue? Venue { get; set; }
        public string EventName { get; set; }
        public DateTime EventDate { get; set; }
        public string Description { get; set; }
    }
}
