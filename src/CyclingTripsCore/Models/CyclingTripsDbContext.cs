using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CyclingTrips.Models
{
    public class CyclingTripsDbContext : IdentityDbContext<CyclingTripsUser>
    {
        private IConfigurationRoot _config;

        public CyclingTripsDbContext(IConfigurationRoot config, DbContextOptions options)
            : base(options)
        {
            _config = config;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer(_config["ConnectionStrings:CyclingTripsContextConnection"]);

        }

        public DbSet<Trip> Trips { get; set; }
        public DbSet<Stop> Stops { get; set; }
        public DbSet<Comment> Comments { get; set; }

    }
}
