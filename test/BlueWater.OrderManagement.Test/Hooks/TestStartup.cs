using BlueWater.OrderManagement.Services;
using BlueWater.OrderManagement.WebApi;
using BlueWater.OrderMangement.DataAccess;
using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using System;

namespace BlueWater.OrderManagement.Test.Hooks
{
    internal class TestStartup : Startup
    {
        private readonly string SqlDatabaseConnection = "SqlDatabaseConnection";

        /// <summary>
        /// Gets serviceCollection
        /// </summary>
        public static IServiceCollection ServiceCollection { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TestStartup"/> class.
        /// </summary>
        /// <param name="configuration">IConfiguration</param>
        public TestStartup(IConfiguration configuration)
            : base(configuration)
        {
            HostingContext.Configuration = configuration;
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddServiceModule();
            services.AddDataModule(Configuration, SqlDatabaseConnection, true); //inMemory DB 

            // memory storage for hangfire
            services.AddHangfire(config => config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                                           .UseSimpleAssemblyNameTypeSerializer()
                                           .UseDefaultTypeSerializer()
                                           .UseMemoryStorage());

            services.AddHangfireServer();

            ServiceCollection = services;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public override void Configure(
            IApplicationBuilder app, IWebHostEnvironment env, IBackgroundJobClient backgroundJobClient,
            IRecurringJobManager recurringJobManager, IServiceProvider serviceProvider, ILogger<Startup> logger)
        {
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            app.UseSerilogRequestLogging();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}