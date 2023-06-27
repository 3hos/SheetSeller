using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;

namespace SheetSeller.Models.Domain
{
    public class DBContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Sheet> Sheets { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DBContext (DbContextOptions<DBContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Sheet>()
                .HasOne(u => u.Author)
                .WithMany(s => s.CreatedSheets);
            modelBuilder.Entity<Sheet>()
                .HasMany(s => s.OwnedBy)
                .WithMany(s => s.OwnedSheets);
            modelBuilder.Entity<Sheet>().Navigation(s => s.OwnedBy).AutoInclude();
            modelBuilder.Entity<Sheet>().Navigation(s => s.Tags).AutoInclude();
            modelBuilder.Entity<Tag>()
                .HasMany(s => s.TaggedSheets)
                .WithMany(t => t.Tags);
        }
    }
}
