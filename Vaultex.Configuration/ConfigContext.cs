using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Vaultex.Configuration.Models;
using Vaultex.Database.Extensions;

namespace Vaultex.Configuration
{
    public class ConfigDbContextFactory : IDesignTimeDbContextFactory<ConfigContext>
    {
        public ConfigContext CreateDbContext(string[] args)
        {
            var optionBuilder = new DbContextOptionsBuilder<ConfigContext>();
            optionBuilder.UseNpgsql();
            return new ConfigContext(optionBuilder.Options);
        }
    }
    public class ConfigContext : DbContext
    {
        public ConfigContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<SettingDbo> Settings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.AddDateTimeConverter();

            modelBuilder.Entity<SettingDbo>(entity =>
            {
                entity
                .HasOne(a => a.Parent)
                .WithMany(b => b.Children)
                .HasForeignKey(nameof(SettingDbo.ParentUuid))
                .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
