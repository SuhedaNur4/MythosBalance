using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MythosBalance.Models;

namespace MythosBalance.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<LifeDomain> LifeDomains { get; set; }
        public DbSet<MythologyGuide> MythologyGuides { get; set; }
        public DbSet<GuideReference> GuideReferences { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<LifeDomain>()
                .HasOne(d => d.MythologyGuide)
                .WithOne(g => g.LifeDomain)
                .HasForeignKey<MythologyGuide>(g => g.LifeDomainId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Activity>()
                .HasOne(a => a.User)
                .WithMany(u => u.Activities)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Activity>()
                .HasOne(a => a.LifeDomain)
                .WithMany(d => d.Activities)
                .HasForeignKey(a => a.LifeDomainId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<GuideReference>()
                .HasOne(r => r.MythologyGuide)
                .WithMany(g => g.References)
                .HasForeignKey(r => r.MythologyGuideId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Notification>()
                .HasOne(n => n.User)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Notification>()
                .HasOne(n => n.LifeDomain)
                .WithMany()
                .HasForeignKey(n => n.LifeDomainId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
