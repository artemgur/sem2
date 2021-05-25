using Microsoft.EntityFrameworkCore;
using DomainModels;
using sem2.DomainModels;

namespace sem2
{
    public class ApplicationContext : DbContext
    {
        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<ImageMetadata> ImageMetadata { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}