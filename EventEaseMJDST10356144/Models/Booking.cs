namespace EventEaseMJDST10356144.Models
{
    public class Booking
    {
        public int Id{ get; set; }
        public int VenueId { get; set; }
        public Venue? Venue { get; set; }
        public int EventId { get; set; }
        public Event? Event { get; set; }
        public DateTime BookingDate { get; set; }
    }
}
