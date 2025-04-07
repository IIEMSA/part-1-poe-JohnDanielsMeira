namespace EventEaseMJDST10356144.Models
{
    public class Venue
    {
        public int Id { get; set; }
        public string VenueName { get; set; }
        public string Location { get; set; }
        public int Capacity { get; set; }
        public string ImageURL { get; set; }

        public List<Booking> Booking { get; set; } = new();
        public List<Event> Events { get; set; } = new();
    }
}
