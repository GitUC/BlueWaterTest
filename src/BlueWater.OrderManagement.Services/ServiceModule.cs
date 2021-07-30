using BlueWater.OrderManagement.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace BlueWater.OrderManagement.Services
{
    public static class ServiceModule
    {
        /// <summary>
        /// Extension method to register service related  
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddServiceModule(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddTransient<IOrderProcess, OrderProcess>();
            services.AddTransient<IProductProcess, ProductProcess>();

            return services;

        }
    }
}
