using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CricBookingsWebsite.Models
{
    [Table("city")]
    public partial class City
    {
        public City()
        {
            Center = new HashSet<Center>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("stateId")]
        public int StateId { get; set; }
        [Required]
        [Column("name")]
        [StringLength(50)]
        public string Name { get; set; }

        [ForeignKey(nameof(StateId))]
        [InverseProperty("City")]
        public virtual State State { get; set; }
        [InverseProperty("City")]
        public virtual ICollection<Center> Center { get; set; }
    }
}
