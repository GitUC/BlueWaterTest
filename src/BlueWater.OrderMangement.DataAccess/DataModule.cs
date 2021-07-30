using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using System;

namespace BlueWater.OrderMangement.DataAccess
{
    public static class DataModule
    {
        public static void AddDataModule( this IServiceCollection serviceCollection, IConfiguration configuration, string connectionStringKey, bool inMemoryDatabase = false)
        {
            if (serviceCollection == null)
            {
                throw new ArgumentNullException(nameof(serviceCollection));
            }

            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            if (string.IsNullOrEmpty(connectionStringKey))
            {
                throw new ArgumentNullException(nameof(connectionStringKey));
            }

            serviceCollection.AddTransient<IProductDataProcess, ProductDataProcess>();
            RegisterContext(serviceCollection, configuration, connectionStringKey, inMemoryDatabase);
        }

        private static void RegisterContext( IServiceCollection serviceCollection, IConfiguration configuration, string connectionStringKey, bool inMemoryDatabase)
        {
            if (inMemoryDatabase)
            {
                serviceCollection.AddDbContext<OrderDataContext>(options => options
                    .UseInMemoryDatabase("BlueWater")
                    .UseLoggerFactory(null));
            }
            else
            {
                serviceCollection.AddDbContext<OrderDataContext>(options => options
                    .UseSqlServer(configuration.GetConnectionString(connectionStringKey))
                    .UseLoggerFactory(null));
            }
        }
    }
}
