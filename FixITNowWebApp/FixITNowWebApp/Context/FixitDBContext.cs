using System.Collections.Generic;
using System.Data.Entity;
using FixITNowWebApp.Models; 

namespace FixITNowWebApp.Context
{
    public class FixitDBContext : DbContext
    {
        public FixitDBContext() : base("FixitConnectionString")
        {
        }

    
        public DbSet<User> Users { get; set; }
        public DbSet<ServiceProvider> ServiceProviders { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<PasswordResetRequest> PasswordResetRequests { get; set; }
    }
}
