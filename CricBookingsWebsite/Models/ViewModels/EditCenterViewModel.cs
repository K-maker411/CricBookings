using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CricBookingsWebsite.Models.ViewModels
{
    public class EditCenterViewModel
    {
        [Required]
        public int cityId { get; set; }
        [Required]
        public int centerId { get; set; }
        [Required]
        public string centerName { get; set; }
        [Required]
        public string addr1 { get; set; }
        public string addr2 { get; set; }
        [Required]
        public string zip { get; set; }

    }
}
