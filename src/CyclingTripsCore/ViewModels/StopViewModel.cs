using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyclingTrips.ViewModels
{
    public class StopViewModel
    {
        [Required]
        [StringLength(255, MinimumLength = 3)]
        public string Location { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        [Required]
        public DateTime Arrival { get; set; }
    }
}
