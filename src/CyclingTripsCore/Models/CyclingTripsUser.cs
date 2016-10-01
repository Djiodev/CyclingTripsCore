using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace CyclingTrips.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class CyclingTripsUser : IdentityUser
    {
        public string BikeType { get; set; }
        public string ProfileImage { get; set; }
    }
}
