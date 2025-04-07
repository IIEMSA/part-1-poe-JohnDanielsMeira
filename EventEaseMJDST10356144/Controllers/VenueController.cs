using EventEaseMJDST10356144.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace EventEaseMJDST10356144.Controllers
{
    public class VenueController : Controller
    {
        private readonly EventEaseDBContext _context;
        public VenueController(EventEaseDBContext context)
        {
            _context = context;
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
        public async Task<IActionResult> Create(Venue venue)
        {


            if (ModelState.IsValid)//Checks if all the fields have filled out correctly before adding new data 
            {

                _context.Add(venue);

                await _context.SaveChangesAsync();

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


            if (venue == null)
            {
                return NotFound();
            }
            return View(venue);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var venue = await _context.Venue.FindAsync(id);
            _context.Venue.Remove(venue);
            await _context.SaveChangesAsync();
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
                    _context.Update(venue);
                    await _context.SaveChangesAsync();
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
    }
}
