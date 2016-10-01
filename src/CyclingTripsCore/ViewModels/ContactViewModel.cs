using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyclingTrips.ViewModels
{
    public class ContactViewModel
    {
        [Required]
        [StringLength(125, MinimumLength = 5)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(125, MinimumLength = 5)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(1024, MinimumLength = 5)]
        public string Message { get; set; }        
    }
}
