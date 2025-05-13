using System.Net.Http.Headers;
using System.Net.Mime;
using EventEaseMJDST10356144.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System.Configuration;


namespace EventEaseMJDST10356144.Controllers
{
    public class VenueController : Controller
    {
        private readonly EventEaseDBContext _context;
        private readonly IConfiguration _configuration;
        public VenueController(EventEaseDBContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public async Task<IActionResult> Index()//Displays the data stored as a table

        {
            var venue = await _context.Venue.ToListAsync();
            return View(venue);
        }

        public IActionResult Create()//Allows users to add new data to the database and the table
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Venue venue)
        {


            if (ModelState.IsValid)//Checks if all the fields have filled out correctly before adding new data 
            {
                if (venue.ImageFile != null)
                {
                    var blobURL = await UploadImageToBlobAsync(venue.ImageFile);
                    venue.ImageURL = blobURL;
                }

                _context.Add(venue);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Venue created successfully.";
                return RedirectToAction(nameof(Index));

            }

            return View(venue);
        }

        public async Task<IActionResult> Details(int? id)//Displays all data that has been added
        {

            var venue = await _context.Venue.FirstOrDefaultAsync(m => m.Id == id);

            if (venue == null)//checks to see that there is data that can be displayed
            {
                return NotFound();
            }
            return View(venue);
        }

        public async Task<IActionResult> Delete(int? id)//Allow user to delete a row of data 
        {
            if (id == null) return NotFound();

            var venue = await _context.Venue.Include(v => v.Events).FirstOrDefaultAsync(m => m.Id == id);
            //.Include(v => v.Events)
            if (venue == null) return NotFound();

            return View(venue);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var venue = await _context.Venue.Include(v => v.Events).FirstOrDefaultAsync(m => m.Id == id); //FindAsync(id);
            if (venue == null) return NotFound();

            var hasBooking = await _context.Booking.AnyAsync(b => b.VenueId == id);
            if (hasBooking)
            {
                TempData["ErrorMessage"] = "Cannot delete venue as it has existing bookings.";
                return RedirectToAction(nameof(Index));
            }

            _context.Venue.Remove(venue);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Venue deleted successfully.";
            return RedirectToAction(nameof(Index));
        }

        private bool VenueExists(int id)//Checks if there is actually a venue that can be edited 
        {
            return _context.Venue.Any(e => e.Id == id);
        }
        public async Task<IActionResult> Edit(int? id)//Allows user to edit data within the tables
        {
            if (id == null)
            {
                return NotFound();
            }

            var venue = await _context.Venue.FindAsync(id);
            if (id == null)
            {
                return NotFound();
            }

            return View(venue);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Venue venue)
        {
            if (id != venue.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (venue.ImageFile != null)
                    {
                        var blobURL = await UploadImageToBlobAsync (venue.ImageFile);
                        venue.ImageURL = blobURL;
                    }
                    else 
                    {
                        //Keep the existing ImageURL
                    }

                    _context.Update(venue);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Venue updated successfully.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VenueExists(venue.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            return View(venue);
        }

        private async Task<string> UploadImageToBlobAsync(IFormFile imageFile)
        {
            var connectionString = _configuration.GetConnectionString("AzureBlobStorage");
            var containerName = "cldv6211poe";

            var blobServiceClient = new BlobServiceClient(connectionString);
            var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(Guid.NewGuid() + Path.GetExtension(imageFile.FileName));

            var blobHttpHeaders = new Azure.Storage.Blobs.Models.BlobHttpHeaders
            {
                ContentType = imageFile.ContentType
            };

            using (var stream = imageFile.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, new Azure.Storage.Blobs.Models.BlobUploadOptions
                {
                    HttpHeaders = blobHttpHeaders
                });
            }

            return blobClient.Uri.ToString();
        }
    }
}
