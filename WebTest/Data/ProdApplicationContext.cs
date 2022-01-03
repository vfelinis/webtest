using Microsoft.EntityFrameworkCore;

namespace WebTest.Data
{
    public class ProdApplicationContext : ApplicationContext
    {
        public ProdApplicationContext(IConfiguration config) : base(config)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseNpgsql(_config.GetConnectionString("DefaultConnection"));
    }
}
