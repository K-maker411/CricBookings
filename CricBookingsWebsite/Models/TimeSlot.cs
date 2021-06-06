using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CricBookingsWebsite.Models
{
    [Table("timeSlot")]
    public partial class TimeSlot
    {
        public TimeSlot()
        {
            Booking = new HashSet<Booking>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("slotDateTime", TypeName = "datetime")]
        public DateTime SlotDateTime { get; set; }
        [Column("centerId")]
        public int CenterId { get; set; }
        [Column("laneId")]
        public int LaneId { get; set; }
        [Column("description")]
        [StringLength(100)]
        public string Description { get; set; }

        [ForeignKey(nameof(CenterId))]
        [InverseProperty("TimeSlot")]
        public virtual Center Center { get; set; }
        [ForeignKey(nameof(LaneId))]
        [InverseProperty("TimeSlot")]
        public virtual Lane Lane { get; set; }
        [InverseProperty("TimeSlot")]
        public virtual ICollection<Booking> Booking { get; set; }
    }
}