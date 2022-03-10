using Microsoft.EntityFrameworkCore;

namespace Banner
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        
        public DbSet<Banner> Banners { get; set; }
    }
}