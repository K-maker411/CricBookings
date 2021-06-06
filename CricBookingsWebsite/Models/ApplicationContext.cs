using CricBookingsWebsite.Models.Configuration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CricBookingsWebsite.Models
{
    public class ApplicationContext : IdentityDbContext<User>
    {
        public ApplicationContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new TestUserConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());

            modelBuilder.Entity<Booking>(entity =>
            {
                entity.Property(e => e.Description).IsUnicode(false);

                entity.HasOne(d => d.TimeSlot)
                    .WithMany(p => p.Booking)
                    .HasForeignKey(d => d.TimeSlotId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_timeSlot_booking");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Booking)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_user_booking");
            });

            modelBuilder.Entity<Center>(entity =>
            {
                entity.Property(e => e.Addr1).IsUnicode(false);

                entity.Property(e => e.Addr2).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.Zip).IsUnicode(false);

                entity.HasOne(d => d.City)
                    .WithMany(p => p.Center)
                    .HasForeignKey(d => d.CityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_city_center");
            });

            modelBuilder.Entity<City>(entity =>
            {
                entity.Property(e => e.Name).IsUnicode(false);

                entity.HasOne(d => d.State)
                    .WithMany(p => p.City)
                    .HasForeignKey(d => d.StateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_state_city");
            });

            modelBuilder.Entity<CricUser>(entity =>
            {
                entity.Property(e => e.Email).IsUnicode(false);

                entity.Property(e => e.FirstName).IsUnicode(false);

                entity.Property(e => e.LastName).IsUnicode(false);

                entity.Property(e => e.MiddleName).IsUnicode(false);

                entity.Property(e => e.Password).IsUnicode(false);

                entity.Property(e => e.PhoneNum).IsUnicode(false);
            });

            modelBuilder.Entity<HolidaySchedule>(entity =>
            {
                entity.Property(e => e.Description).IsUnicode(false);

                entity.HasOne(d => d.Center)
                    .WithMany(p => p.HolidaySchedule)
                    .HasForeignKey(d => d.CenterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_center_holidaySchedule");
            });

            modelBuilder.Entity<Lane>(entity =>
            {
                entity.Property(e => e.Name).IsUnicode(false);

                entity.HasOne(d => d.Center)
                    .WithMany(p => p.Lane)
                    .HasForeignKey(d => d.CenterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_center_lane");
            });

            //modelBuilder.Entity<Schedule>(entity =>
            //{
            //    entity.HasOne(d => d.Center)
            //        .WithMany(p => p.Schedule)
            //        .HasForeignKey(d => d.CenterId)
            //        .OnDelete(DeleteBehavior.ClientSetNull)
            //        .HasConstraintName("FK_center_schedule");
            //});

            modelBuilder.Entity<Schedule>(entity =>
            {
                entity.ToTable("schedule");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CenterId).HasColumnName("centerId");

                entity.Property(e => e.DayOfWeek).HasColumnName("dayOfWeek");

                entity.Property(e => e.EndTime).HasColumnName("endTime");

                entity.Property(e => e.StartTime).HasColumnName("startTime");

                entity.HasOne(d => d.Center)
                    .WithMany(p => p.Schedule)
                    .HasForeignKey(d => d.CenterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_center_schedule");
            });

            modelBuilder.Entity<State>(entity =>
            {
                entity.HasComment("state names");

                entity.Property(e => e.Abbr).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);
            });

            modelBuilder.Entity<TimeSlot>(entity =>
            {
                entity.Property(e => e.Description).IsUnicode(false);

                entity.HasOne(d => d.Center)
                    .WithMany(p => p.TimeSlot)
                    .HasForeignKey(d => d.CenterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_center_timeSlot");

                entity.HasOne(d => d.Lane)
                    .WithMany(p => p.TimeSlot)
                    .HasForeignKey(d => d.LaneId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_lane_timeSlot");
            });
        }

        public DbSet<TestUser> TestUsers { get; set; }

        public virtual DbSet<Booking> Booking { get; set; }
        public virtual DbSet<Center> Center { get; set; }
        public virtual DbSet<City> City { get; set; }
        public virtual DbSet<CricUser> CricUser { get; set; }
        public virtual DbSet<HolidaySchedule> HolidaySchedule { get; set; }
        public virtual DbSet<Lane> Lane { get; set; }
        public virtual DbSet<Schedule> Schedule { get; set; }
        public virtual DbSet<State> State { get; set; }
        public virtual DbSet<TimeSlot> TimeSlot { get; set; }
    }
}
