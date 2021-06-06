using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CricBookingsWebsite.Models.Configuration
{
    public class TestUserConfiguration : IEntityTypeConfiguration<TestUser>
    {
        public void Configure(EntityTypeBuilder<TestUser> builder)
        {
            builder.HasData(
                new TestUser
                {
                    Id = 40,
                    FirstName = "Bobby",
                    LastName = "Bob",
                    EmailAddress = "bobby@bob.com",
                    PhoneNumber = "1234",
                    Password = "abcd",
                    
                },
                
                new TestUser
                {
                    Id = 45,
                    FirstName = "Babu",
                    LastName = "Khan",
                    EmailAddress = "babu@khan.com",
                    PhoneNumber = "5678",
                    Password = "efgh"
                }
                
                );
        }
    }
}
