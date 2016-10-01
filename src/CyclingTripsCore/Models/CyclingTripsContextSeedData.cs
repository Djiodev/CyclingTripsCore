using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyclingTrips.Models
{
    public class CyclingTripsContextSeedData
    {
        private CyclingTripsDbContext _context;
        private IHostingEnvironment _hostingEnvironment;
        private UserManager<CyclingTripsUser> _userManager;

        public CyclingTripsContextSeedData(CyclingTripsDbContext context, UserManager<CyclingTripsUser> userManager, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task EnsureSeedData()
        {
            if (await _userManager.FindByEmailAsync("g.zarrella@hotmail.it") == null)
            {
                var newUser = new CyclingTripsUser()
                {
                    BikeType = "Road_Bike",
                    UserName = "Djio",
                    Email = "g.zarrella@hotmail.it",
                    ProfileImage = _hostingEnvironment.WebRootPath + "\\images\\Djio.jpg"
                };
                await _userManager.CreateAsync(newUser, "P@ssw0rd");

            }

            if (await _userManager.FindByEmailAsync("user@ctrips.com") == null)
            {
                var newUser = new CyclingTripsUser()
                {
                    BikeType = "Road_Bike",
                    UserName = "User",
                    Email = "user@ctrips.com",
                    ProfileImage = _hostingEnvironment.WebRootPath + "\\images\\User.jpg"
                };
                await _userManager.CreateAsync(newUser, "P@ss!w0rd");

            }


            if (!_context.Trips.Any())
            {
                var ParisToLondon = new Trip()
                {
                    Name = "Paris to London",
                    Created = DateTime.UtcNow,
                    Username = "Djio",
                    Stops = new List<Stop>()
                    {
                        new Stop() {Arrival = new DateTime(2014, 8, 31), Location = "Paris", Latitude = 48.856614, Longitude = 2.3522219 },
                        new Stop() {Arrival = new DateTime(2014, 9, 2), Location = "Dieppe", Latitude = 49.922992, Longitude = 1.077483 },
                        new Stop() {Arrival = new DateTime(2014, 9, 4), Location = "London", Latitude = 51.5073509, Longitude = -0.1277583 }
                    }
                };

                _context.Trips.Add(ParisToLondon);
                _context.Stops.AddRange(ParisToLondon.Stops);

                var Irpinia = new Trip()
                {
                    Name = "Irpinia",
                    Created = DateTime.UtcNow,
                    Username = "User",
                    Stops = new List<Stop>()
                    {
                        new Stop() {Arrival = new DateTime(2015, 7, 11), Location = "Gesualdo", Latitude = 41.006681, Longitude = 15.0699976 },
                        new Stop() {Arrival = new DateTime(2015, 7, 12), Location = "Lago Laceno", Latitude = 40.806672, Longitude = 15.0967176 },
                        new Stop() {Arrival = new DateTime(2015, 7, 13), Location = "Sant'Angelo dei Lombardi", Latitude = 40.9277972, Longitude = 15.1777683 }
                    },
                    Comments = new List<Comment>()
                    {
                        new Comment() { Body = "Amazing Trip", Username = "Djio", Created = new DateTime(2015, 2, 3)},
                        new Comment() { Body = "Loved the hilly journey", Username = "User", Created = new DateTime(2015, 3, 5)}
                    }
                };

                _context.Trips.Add(Irpinia);
                _context.Stops.AddRange(Irpinia.Stops);
                _context.Comments.AddRange(Irpinia.Comments);

                await _context.SaveChangesAsync();

            }

        }
    }
}
