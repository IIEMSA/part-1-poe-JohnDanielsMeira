using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventEaseMJDST10356144.Models
{
    public class Venue
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Venue name is required.")]
        [StringLength(100, ErrorMessage = "Venue name cannot exceed 100 characters.")]
        public string VenueName { get; set; }

        [Required(ErrorMessage = "Location is required.")]
        [StringLength(150, ErrorMessage = "Location cannot exceed 150 characters.")]
        public string Location { get; set; }

        [Range(1, 10000, ErrorMessage = "Capacity must be between 1 and 10,000.")]
        public int Capacity { get; set; }

        //This stays - to store the URL of the image uploaded
        public string? ImageURL { get; set; }

        public List<Booking> Booking { get; set; } = new();
        public List<Event> Events { get; set; } = new();

        //Add this new one - only for uploading from the Create/Edit form
        [NotMapped]
        public IFormFile? ImageFile { get; set; }
    }
}
