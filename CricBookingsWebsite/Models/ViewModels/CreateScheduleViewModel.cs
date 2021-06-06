using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CricBookingsWebsite.Models.ViewModels
{
    public class CreateScheduleViewModel
    {
        public int centerId { get; set; }
        public short dayOfWeek { get; set; }
        public int startTimeHours { get; set; }
        public int startTimeMinutes { get; set; }
        public int endTimeHours { get; set; }
        public int endTimeMinutes { get; set; }
    }
}
