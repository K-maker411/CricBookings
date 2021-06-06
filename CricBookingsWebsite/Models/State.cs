using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CricBookingsWebsite.Models
{
    [Table("state")]
    public partial class State
    {
        public State()
        {
            City = new HashSet<City>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Required]
        [Column("name")]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        [Column("abbr")]
        [StringLength(50)]
        public string Abbr { get; set; }

        [InverseProperty("State")]
        public virtual ICollection<City> City { get; set; }
    }
}
