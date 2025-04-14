using Microsoft.EntityFrameworkCore;
using FIXITNOWWEBAPI.Models;

public class FixItNowDbContext : DbContext
{
    public FixItNowDbContext(DbContextOptions<FixItNowDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Service> Services { get; set; }
    public DbSet<FIXITNOWWEBAPI.Models.ServiceProvider> ServiceProviders { get; set; }
    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<UserProfile> UserProfiles { get; set; }
    public DbSet<PasswordResetRequest> PasswordResetRequests { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        base.OnModelCreating(modelBuilder); 
    }
}
