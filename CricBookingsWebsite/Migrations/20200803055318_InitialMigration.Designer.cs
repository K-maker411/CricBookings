﻿// <auto-generated />
using CricBookingsWebsite.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CricBookingsWebsite.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20200803055318_InitialMigration")]
    partial class InitialMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CricBookingsWebsite.Models.TestUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("EmailAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MiddleName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("TestUsers");

                    b.HasData(
                        new
                        {
                            Id = 40,
                            EmailAddress = "bobby@bob.com",
                            FirstName = "Bobby",
                            LastName = "Bob",
                            Password = "abcd",
                            PhoneNumber = "1234"
                        },
                        new
                        {
                            Id = 45,
                            EmailAddress = "babu@khan.com",
                            FirstName = "Babu",
                            LastName = "Khan",
                            Password = "efgh",
                            PhoneNumber = "5678"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}