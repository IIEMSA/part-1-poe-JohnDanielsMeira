using EventEaseMJDST10356144.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EventEaseMJDST10356144.Controllers
{
    public class EventController : Controller
    {
        private readonly EventEaseDBContext _context;
        public EventController(EventEaseDBContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()//Displays the data stored as a table
        {
            var events = await _context.Event.Include(e => e.Venue).ToListAsync();
            return View(events);
        }

        public IActionResult Create()//Allows users to add new data to the database and the table
        {
            ViewBag.VenueId = new SelectList(_context.Venue, "Id", "VenueName");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Event events)
        {
            if (ModelState.IsValid)//Checks if all the fields have filled out correctly before adding new data 
            {
                _context.Add(events);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.VenueId = new SelectList(_context.Venue, "Id", "VenueName", events.VenueId);//Foreign key is displayed and can be selcetd by user
            return View(events);
        }

        public async Task<IActionResult> Details(int? id)//Displays all data that has been added including foreign key VenueId
        {

            var events = await _context.Event.Include(e => e.Venue).FirstOrDefaultAsync(m => m.Id == id);

            if (events == null)//checks to see that there is data that can be displayed
            {
                return NotFound();
            }
            return View(events);
        }

        public async Task<IActionResult> Delete(int? id)//Allow user to delete a row of data 
        {
            var events = await _context.Event.FirstOrDefaultAsync(m => m.Id == id);


            if (events == null)
            {
                return NotFound();
            }
            return View(events);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var events = await _context.Event.FindAsync(id);
            _context.Event.Remove(events);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventExists(int id)//Checks if there is actually a event that can be edited 
        {
            return _context.Event.Any(e => e.Id == id);
        }
        public async Task<IActionResult> Edit(int? id)//Allows user to edit data within the tables
        {
            if (id == null)
            {
                return NotFound();
            }

            var events = await _context.Event.FindAsync(id);
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.VenueId = new SelectList(_context.Venue, "Id", "VenueName", events.VenueId);
            return View(events);
        }
        [HttpPost]

        public async Task<IActionResult> Edit(int id, Event events)
        {
            if (id != events.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(events);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(events.Id))
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

            return View(events);
        }
    }
}
