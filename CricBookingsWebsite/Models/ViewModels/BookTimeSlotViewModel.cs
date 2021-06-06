using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CricBookingsWebsite.Models.ViewModels
{
    public class BookTimeSlotViewModel
    {
        public int stateId { get; set; }
        public string stateName { get; set; }
        public int cityId { get; set; }
        public string cityName { get; set; }
        public int centerId { get; set; }
        public string centerName { get; set; }
        public string chosenDate { get; set; }

        // this will change eventually
        public int duration { get; set; } = 60;
    }
}
