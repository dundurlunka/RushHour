namespace RushHour.Data
{
    using Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class RushHourDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public RushHourDbContext(DbContextOptions<RushHourDbContext> options) : base(options)
        {
        }

        public DbSet<Appointment> Appointments { get; set; }

        public DbSet<Activity> Activities { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Activity>()
                .HasIndex(a => a.Name)
                .IsUnique();

            modelBuilder
                .Entity<Appointment>()
                .HasIndex(a => new { a.Title, a.StartDateTime })
                .IsUnique();

            modelBuilder
                .Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder
                .Entity<User>()
                .HasMany(u => u.Appointments)
                .WithOne(a => a.User)
                .HasForeignKey(a => a.UserId);

            modelBuilder
                .Entity<ActivityAppointment>()
                .HasKey(aa => new { aa.ActivityId, aa.AppointmentId });

            modelBuilder
                .Entity<ActivityAppointment>()
                .HasOne(aa => aa.Activity)
                .WithMany(act => act.Appointments)
                .HasForeignKey(aa => aa.ActivityId);

            modelBuilder
                .Entity<ActivityAppointment>()
                .HasOne(aa => aa.Appointment)
                .WithMany(app => app.Activities)
                .HasForeignKey(aa => aa.AppointmentId);

            base.OnModelCreating(modelBuilder);
        }        
    }
}
