using Microsoft.EntityFrameworkCore;
using WebTest.Data.Models;

namespace WebTest.Data
{
    public class ApplicationContext: DbContext
    {
        protected readonly IConfiguration _config;
        public ApplicationContext(IConfiguration config)
        {
            _config = config;
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("Users");
        }
    }
}
