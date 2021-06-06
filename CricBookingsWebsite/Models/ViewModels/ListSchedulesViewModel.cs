using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CricBookingsWebsite.Models.ViewModels
{
    public class ListSchedulesViewModel
    {
        public int id { get; set; }
        public int centerId { get; set; }
        public string dayOfWeek { get; set; }
        public int startHours { get; set; }
        public int startMinutes { get; set; }
        public int endHours { get; set; }
        public int endMinutes { get; set; }

    }
}
