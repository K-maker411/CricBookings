using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CricBookingsWebsite.Models
{
    [Table("cricUser")]
    public partial class CricUser
    {
        public CricUser()
        {
            Booking = new HashSet<Booking>();
        }

        [Key]
        [Column("id")]
        public string Id { get; set; }
        [Required]
        [Column("firstName")]
        [StringLength(100)]
        public string FirstName { get; set; }
        [Column("middleName")]
        [StringLength(50)]
        public string MiddleName { get; set; }
        [Required]
        [Column("lastName")]
        [StringLength(100)]
        public string LastName { get; set; }
        [Required]
        [Column("email")]
        [StringLength(200)]
        public string Email { get; set; }
        [Required]
        [Column("phoneNum")]
        [StringLength(20)]
        public string PhoneNum { get; set; }
        [Required]
        [Column("password")]
        [StringLength(1024)]
        public string Password { get; set; }

        [InverseProperty("User")]
        public virtual ICollection<Booking> Booking { get; set; }
    }
}
