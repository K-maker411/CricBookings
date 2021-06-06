using System;
using System.Collections.Generic;

#nullable disable

namespace CricBookingsWebsite.Models
{
    public partial class TestUser
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
    }
}
