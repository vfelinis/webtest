using Microsoft.EntityFrameworkCore;

namespace WebTest.Data
{
    public class DevApplicationContext: ApplicationContext
    {
        public DevApplicationContext(IConfiguration config) : base(config)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite(_config.GetConnectionString("DefaultConnection"));
    }
}
