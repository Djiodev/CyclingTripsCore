using System.Linq;
using Microsoft.AspNetCore.Mvc;
using CyclingTrips.Models;
using Microsoft.EntityFrameworkCore;

namespace CyclingTrips.Controllers
{
    public class BrowseController : Controller
    {
        private ICyclingTripsRepository _repository;
        private CyclingTripsDbContext _context;

        public BrowseController(CyclingTripsDbContext context, ICyclingTripsRepository repository)
        {
            _repository = repository;
            _context = context;   
        }

        // GET: Trip
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Trip(int? id)
        {
            var trip = _context.Trips
                .Include(t => t.Stops)
                .Where(t => t.ID == id)
                .FirstOrDefault();
            return View(trip);
        }

        // GET: Trip/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Trip trip = _repository.GetAllTrips().Single(m => m.ID == id);
            if (trip == null)
            {
                return NotFound();
            }

            return View(trip);
        }
    }
}
