using Microsoft.EntityFrameworkCore;
using TheShortinator.Models;

namespace TheShortinator.Data
{
    public class URLDBContext : DbContext
    {
        public URLDBContext()
    : base()
        { }
        public URLDBContext(DbContextOptions<URLDBContext> options)
            : base(options)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=URLS;Integrated Security=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) { }
        public DbSet<ShortinatorURL> ShortinatorURLs { get; set; }
    }
}
