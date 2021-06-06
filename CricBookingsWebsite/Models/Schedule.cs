using System;
using System.Collections.Generic;

#nullable disable

namespace CricBookingsWebsite.Models
{
    public partial class Schedule
    {
        public int Id { get; set; }
        public int CenterId { get; set; }
        public int StartTime { get; set; }
        public int EndTime { get; set; }
        public short DayOfWeek { get; set; }

        public virtual Center Center { get; set; }
    }
}
