using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using librarymenagment.Models;

namespace librarymenagment.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<librarymenagment.Models.Category> Category { get; set; } = default!;
        public DbSet<librarymenagment.Models.Author> Author { get; set; } = default!;
        public DbSet<librarymenagment.Models.Book> Book { get; set; } = default!;

        public override int SaveChanges()
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is BaseModel && (
                    e.State == EntityState.Added 
                    || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                var entity = (BaseModel)entityEntry.Entity;

                if (entityEntry.State == EntityState.Added)
                {
                    entity.CreatedAt = DateTime.UtcNow;
                }
                else if (entityEntry.State == EntityState.Modified)
                {
                    entity.UpdatedAt = DateTime.UtcNow;
                }
            }

            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is BaseModel && (
                    e.State == EntityState.Added 
                    || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                var entity = (BaseModel)entityEntry.Entity;

                if (entityEntry.State == EntityState.Added)
                {
                    entity.CreatedAt = DateTime.UtcNow;
                }
                else if (entityEntry.State == EntityState.Modified)
                {
                    entity.UpdatedAt = DateTime.UtcNow;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
        public DbSet<Coments> Coments { get; set; } = default!;

        public DbSet<PublishingHouse> PublishingHouses {  get; set; }
        public DbSet<librarymenagment.Models.Available> Available { get; set; } = default!;
        public DbSet<UserBookPermission> UserBookPermission { get; set; }
        public DbSet<librarymenagment.Models.Member> Member { get; set; } = default!;
    }
}
