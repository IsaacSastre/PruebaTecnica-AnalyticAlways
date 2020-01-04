using Microsoft.EntityFrameworkCore;

namespace PTAnalitic.Infrastructure
{
    public class PTDbContext : DbContext
    {
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