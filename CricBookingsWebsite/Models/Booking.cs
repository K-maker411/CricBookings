using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CricBookingsWebsite.Models
{
    [Table("booking")]
    public partial class Booking
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("userId")]
        public string UserId { get; set; }
        [Column("timeSlotId")]
        public int TimeSlotId { get; set; }
        [Column("description")]
        [StringLength(100)]
        public string Description { get; set; }

        [ForeignKey(nameof(TimeSlotId))]
        [InverseProperty("Booking")]
        public virtual TimeSlot TimeSlot { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty(nameof(CricUser.Booking))]
        public virtual CricUser User { get; set; }
    }
}
