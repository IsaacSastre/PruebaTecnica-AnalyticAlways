using Microsoft.EntityFrameworkCore;
using PTAnalitic.Core.Model;

namespace PTAnalitic.Infrastructure
{
    public class PTDbContext : DbContext
    {
        public DbSet<ProductHistory> ProductHistory { get; set; }
        
        public PTDbContext(DbContextOptions<PTDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }
    }
}