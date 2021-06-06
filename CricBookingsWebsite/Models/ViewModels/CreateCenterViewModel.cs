using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace CricBookingsWebsite.Models.ViewModels
{
    public class CreateCenterViewModel
    {
        [Required]
        public string StateName { get; set; }
        public int StateId { get; set; }
        public IEnumerable<SelectListItem> States { get; set; }
        [Required]
        public string CityName { get; set; }
        public int CityId { get; set; }
        public IEnumerable<SelectListItem> Cities { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        [Required]
        public string Zip { get; set; }

    }
}
