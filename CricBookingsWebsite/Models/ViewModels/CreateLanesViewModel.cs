using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CricBookingsWebsite.Models.ViewModels
{
    public class CreateLanesViewModel
    {

        public int centerId { get; set; }
        [Required]
        public string laneName { get; set; }
        [Required]
        public bool isInactive { get; set; }
    }
}
