using CustomerServicesSystem.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CustomerServicesSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<CallCenterRecord> CallCenterRecords { get; set; }
        public DbSet<CallPurpose>      CallPurposes      { get; set; }
        public DbSet<VisitType>        VisitTypes        { get; set; }
        public DbSet<OutcomeOfCall>    OutcomesOfCall    { get; set; }
        public DbSet<Doctor>           Doctors           { get; set; }
        public DbSet<Department>       Departments       { get; set; }
        public DbSet<BookedStatus>     BookedStatuses    { get; set; }
        public DbSet<StaffMember>      StaffMembers      { get; set; }
        public DbSet<Source>           Sources           { get; set; }
        public DbSet<Nationality>      Nationalities     { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Seed lookups
            builder.Entity<CallPurpose>().HasData(
                new CallPurpose { Id=1, Name="Book appointment" },
                new CallPurpose { Id=2, Name="Offer" },
                new CallPurpose { Id=3, Name="Service Price" },
                new CallPurpose { Id=4, Name="Follow Up" },
                new CallPurpose { Id=5, Name="Complaint" },
                new CallPurpose { Id=6, Name="Inquiry" }
            );
            builder.Entity<VisitType>().HasData(
                new VisitType { Id=1, Name="New" },
                new VisitType { Id=2, Name="Revisit" }
            );
            builder.Entity<OutcomeOfCall>().HasData(
                new OutcomeOfCall { Id=1, Name="Outdoor Marketing" },
                new OutcomeOfCall { Id=2, Name="WhatsApp" },
                new OutcomeOfCall { Id=3, Name="Phone Call" },
                new OutcomeOfCall { Id=4, Name="Walk In" },
                new OutcomeOfCall { Id=5, Name="Online" }
            );
            builder.Entity<BookedStatus>().HasData(
                new BookedStatus { Id=1, Name="Yes" },
                new BookedStatus { Id=2, Name="No" },
                new BookedStatus { Id=3, Name="Pending" }
            );
            builder.Entity<Nationality>().HasData(
                new Nationality { Id=1, Name="Kuwaiti" },
                new Nationality { Id=2, Name="Non-Kuwaiti" },
                new Nationality { Id=3, Name="Saudi" },
                new Nationality { Id=4, Name="Egyptian" },
                new Nationality { Id=5, Name="Indian" },
                new Nationality { Id=6, Name="Filipino" },
                new Nationality { Id=7, Name="Other" }
            );
        }
    }
}
