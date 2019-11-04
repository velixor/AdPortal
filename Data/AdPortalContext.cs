using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class AdPortalContext : DbContext
    {
        public AdPortalContext(DbContextOptions<AdPortalContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Ad> Ads { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ad>()
                .Property(ad => ad.Rating)
                .HasDefaultValue(0);
            modelBuilder.Entity<Ad>()
                .Property(ad => ad.Number)
                .UseIdentityColumn();
            modelBuilder.Entity<Ad>()
                .HasIndex(ad => ad.Number);

            modelBuilder.Entity<User>()
                .HasMany(user => user.Ads)
                .WithOne(ad => ad.User)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<User>()
                .HasIndex(user => user.Email)
                .IsUnique();
                
        }
    }
}