using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Configuration
{
    public class ConfigurationService
    {
        private readonly ConfigurationContext _dbContext;

        public ConfigurationService(ConfigurationContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<ConfigurationItem>> GetConfiugationValueListAsync()
        {
            return await _dbContext.ConfigurationItems.ToListAsync().ConfigureAwait(false);
        }

        public async Task UpdateConfigurationItemListAsync(List<ConfigurationItem> configurationValueList)
        {
            _dbContext.ConfigurationItems.UpdateRange(configurationValueList);
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
