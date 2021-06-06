using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CricBookingsWebsite.Models
{
    [Table("center")]
    public partial class Center
    {
        public Center()
        {
            HolidaySchedule = new HashSet<HolidaySchedule>();
            Lane = new HashSet<Lane>();
            Schedule = new HashSet<Schedule>();
            TimeSlot = new HashSet<TimeSlot>();

        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("cityId")]
        public int CityId { get; set; }
        [Required]
        [Column("name")]
        [StringLength(100)]
        public string Name { get; set; }
        [Required]
        [Column("addr1")]
        [StringLength(100)]
        public string Addr1 { get; set; }
        [Column("addr2")]
        [StringLength(100)]
        public string Addr2 { get; set; }
        [Required]
        [Column("zip")]
        [StringLength(10)]
        public string Zip { get; set; }

        [ForeignKey(nameof(CityId))]
        [InverseProperty("Center")]
        public virtual City City { get; set; }
        [InverseProperty("Center")]
        public virtual ICollection<HolidaySchedule> HolidaySchedule { get; set; }
        [InverseProperty("Center")]
        public virtual ICollection<Lane> Lane { get; set; }
        [InverseProperty("Center")]
        public virtual ICollection<Schedule> Schedule { get; set; }
        [InverseProperty("Center")]
        public virtual ICollection<TimeSlot> TimeSlot { get; set; }
    }
}
