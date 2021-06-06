using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CricBookingsWebsite.Models
{
    [Table("lane")]
    public partial class Lane
    {
        public Lane()
        {
            TimeSlot = new HashSet<TimeSlot>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("centerId")]
        public int CenterId { get; set; }
        [Required]
        [Column("name")]
        [StringLength(100)]
        public string Name { get; set; }
        [Column("inactive")]
        public bool Inactive { get; set; }

        [ForeignKey(nameof(CenterId))]
        [InverseProperty("Lane")]
        public virtual Center Center { get; set; }
        [InverseProperty("Lane")]
        public virtual ICollection<TimeSlot> TimeSlot { get; set; }
    }
}
