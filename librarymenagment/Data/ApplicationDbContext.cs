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
    }
}
