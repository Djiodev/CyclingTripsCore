using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyclingTrips.ViewModels
{
    public class CommentViewModel
    {
        [Required]
        [StringLength(1024, MinimumLength = 5)]
        public string Body { get; set; }
        [Required]
        public string Username { get; set; }
        public string UserProfileImg { get; set; }
    }
}
