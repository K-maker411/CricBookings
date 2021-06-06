using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CricBookingsWebsite.Models
{
    [Table("holidaySchedule")]
    public partial class HolidaySchedule
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("centerId")]
        public int CenterId { get; set; }
        [Column("hdate", TypeName = "datetime")]
        public DateTime Hdate { get; set; }
        [Column("description")]
        [StringLength(100)]
        public string Description { get; set; }

        [ForeignKey(nameof(CenterId))]
        [InverseProperty("HolidaySchedule")]
        public virtual Center Center { get; set; }
    }
}
