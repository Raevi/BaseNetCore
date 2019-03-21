using Microsoft.EntityFrameworkCore;

namespace Application.Configuration
{
    public class ConfigurationContext : DbContext
    {
        public ConfigurationContext(DbContextOptions<ConfigurationContext> options) : base(options)
        {
        }

        public DbSet<ConfigurationItem> ConfigurationItems { get; set; }
    }
}
