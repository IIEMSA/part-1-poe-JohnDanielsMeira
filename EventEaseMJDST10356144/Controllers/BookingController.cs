using EventEaseMJDST10356144.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EventEaseMJDST10356144.Controllers
{
    public class BookingController : Controller
    {
        private readonly EventEaseDBContext _context;
        public BookingController(EventEaseDBContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index() //Displays the data stored as a table
        {
            var booking = await _context.Booking.Include(b => b.Venue).Include(b => b.Event).ToListAsync();
            return View(booking);
        }

        public IActionResult Create()//Allows users to add new data to the database and the table
        {
            ViewBag.VenueId = new SelectList(_context.Venue, "Id", "VenueName");//Foreign key is displayed and can be selcetd by user
            ViewBag.EventId = new SelectList(_context.Event, "Id", "EventName");//^^
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Booking booking)
        {
            if (ModelState.IsValid)//Checks if all the fields have filled out correctly before adding new data 
            {
                _context.Add(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.VenueId = new SelectList(_context.Venue, "Id", "VenueName", booking.VenueId);
            ViewBag.EventId = new SelectList(_context.Event, "Id", "EventName", booking.EventId);
            return View(booking);
        }

        public async Task<IActionResult> Details(int? id)//Displays all data that has been added including foreign keys VenueId and EventId
        {

            var booking = await _context.Booking.Include(b => b.Venue).Include(b => b.Event).FirstOrDefaultAsync(m => m.Id == id);

            if (booking == null)//checks to see that there is data that can be displayed
            {
                return NotFound();
            }
            return View(booking);
        }

        //Retreives data to allow user to delete
        public async Task<IActionResult> Delete(int? id)//Allow user to delete a row of data 
        {
            var booking = await _context.Booking.FirstOrDefaultAsync(m => m.Id == id);


            if (booking == null)
            {
                return NotFound();
            }
            return View(booking);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var booking = await _context.Booking.FindAsync(id);
            _context.Booking.Remove(booking);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookingExists(int id)//Checks if there is actually a booking that can be edited 
        {
            return _context.Booking.Any(e => e.Id == id);
        }
        public async Task<IActionResult> Edit(int? id)//Allows user to edit data within the tables
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Booking.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            ViewBag.VenueId = new SelectList(_context.Venue, "Id", "VenueName", booking.VenueId);
            ViewBag.EventId = new SelectList(_context.Event, "Id", "EventName", booking.EventId);
            return View(booking);
        }

        [HttpPost]

        public async Task<IActionResult> Edit(int id, Booking booking)
        {
            if (id != booking.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(booking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExists(booking.Id))
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
            ViewBag.VenueId = new SelectList(_context.Venue, "Id", "VenueName", booking.VenueId);
            ViewBag.EventId = new SelectList(_context.Event, "Id", "EventName", booking.EventId);
            return View(booking);
        }
    }
}
