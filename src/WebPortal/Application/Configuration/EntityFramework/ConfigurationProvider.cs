using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Application.Configuration
{
    public class ConfigurationProvider : Microsoft.Extensions.Configuration.ConfigurationProvider
    {
        private readonly List<ConfigurationItem> ItemNameList = new List<ConfigurationItem>(){
           new ConfigurationItem(){ Id = "Item1", Value="Value1" },
           new ConfigurationItem(){ Id = "Item2", Value="Value2" }
        };

        private Action<DbContextOptionsBuilder> OptionsAction { get; }

        public ConfigurationProvider(Action<DbContextOptionsBuilder> optionsAction)
        {
            OptionsAction = optionsAction;
        }

        public override void Load()
        {
            var builder = new DbContextOptionsBuilder<ConfigurationContext>();

            OptionsAction(builder);

            using (var dbContext = new ConfigurationContext(builder.Options))
            {
                InitDatabase(dbContext);
            }
        }

        public void InitDatabase(ConfigurationContext dbContext)
        {
            CreateTableIfNotExist(dbContext);
            EnsureItemsAreCreated(dbContext);

            Data = dbContext.ConfigurationItems.ToDictionary(c => c.Id, c => c.Value);
        }



        private void CreateTableIfNotExist(ConfigurationContext dbContext)
        {
            try
            {
                int count = dbContext.ConfigurationItems.Count();
            }
            catch (Exception)
            {
                CreateSqlServerTable(dbContext);
            }
        }

        private void CreateSqlServerTable(ConfigurationContext dbContext)
        {
            string query = $@"
                    CREATE TABLE [Configuration].[{dbContext.Database.ProviderName}] (
                        [Id] nvarchar(450) NOT NULL,
                        [Value] nvarchar(max) NULL,
                        CONSTRAINT [PK_AppConfiguration] PRIMARY KEY ([Id])
                    )";

            dbContext.Database.ExecuteSqlCommand(new RawSqlString(query));
        }

        private void EnsureItemsAreCreated(ConfigurationContext dbContext)
        {
            bool updateRequired = false;
            List<string> optionInDb = dbContext.ConfigurationItems.Select(o => o.Id).ToList();
            foreach (ConfigurationItem item in ItemNameList)
            {
                if (!optionInDb.Contains(item.Id))
                {
                    dbContext.ConfigurationItems.Add(item);
                    updateRequired = true;
                }
            }

            if (updateRequired == true)
            {
                dbContext.SaveChanges();
            }
        }
    }
}
