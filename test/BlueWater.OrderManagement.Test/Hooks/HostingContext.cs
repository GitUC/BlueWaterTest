using BlueWater.OrderManagement.Common.Contracts;
using BlueWater.OrderMangement.DataAccess;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;

namespace BlueWater.OrderManagement.Test.Hooks
{
    [Binding]
    public static class HostingContext
    {

        /// <summary>
        /// Connection string key
        /// </summary>
        public const string ConnectionStringKey = "SqlDatabaseConnection";

        internal static IConfiguration Configuration { get; set; }
        
        /// <summary>
        /// Gets microsoft Test Server
        /// </summary>
        internal static TestServer Server { get; private set; }

      
        /// <summary>
        /// payload for Produt
        /// </summary>
        internal static ProductPayload ProductPayload { get; set; } 

        internal static ProductDto Product { get; set; }

        public static bool UseInMemoryDb
        {
            get
            {
                return (bool)Configuration.GetValue(typeof(bool), "InMemoryDbUse");
            }
        }


        /// <summary>
        /// Initial Setup Before the whole component tests run
        /// </summary>
        [BeforeTestRun]
        public static void Setup()
        {
            // Workaround
            var builder = new WebHostBuilder()
              .ConfigureAppConfiguration((context, configBuilder) =>
              {
                  IWebHostEnvironment env = context.HostingEnvironment;
                  configBuilder
                      .AddJsonFile("AppSettings.json", optional: false, reloadOnChange: true)
                      .AddEnvironmentVariables();
              })
              .UseSerilog()
              .UseStartup<TestStartup>();


            Server = new TestServer(builder);
            if (!UseInMemoryDb)
            {
                InitializeDatabase();
            }
        }

        /// <summary>
        /// Clear all resources before the whole tests end
        /// </summary>
        [AfterTestRun]
        public static void Teardown()
        {
            if (Server != null)
            {
                Server.Dispose();
                Server = null;
            }
        }

        /// <summary>
        /// A setup for each scenario test
        /// </summary>
        [BeforeScenario]
        public static void ScenarioSetup()
        {
            if (!UseInMemoryDb)
            {
                CleanupDatabase();
            }
        }

        private static void CleanupDatabase()
        {
            var DatabaseConnectionString = Configuration.GetConnectionString("SqlDatabaseConnection");

            using (var context = new OrderDataContext(new DbContextOptionsBuilder<OrderDataContext>()
                    .UseSqlServer(DatabaseConnectionString)
                    .UseLoggerFactory(null)
                    .Options))
            {
                context.Products.RemoveRange(context.Products);
                context.SaveChanges();
            }
        }

        private static void InitializeDatabase()
        {
            var DatabaseConnectionString = Configuration.GetConnectionString("SqlDatabaseConnection");

            using (var context = new OrderDataContext(new DbContextOptionsBuilder<OrderDataContext>()
                    .UseSqlServer(DatabaseConnectionString)
                    .UseLoggerFactory(null)
                    .Options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
               // context.Database.ExecuteSqlCommand("ALTER DATABASE CURRENT SET ALLOW_SNAPSHOT_ISOLATION ON");
               // context.Database.ExecuteSqlCommand("ALTER DATABASE CURRENT SET READ_COMMITTED_SNAPSHOT ON");
            }
        }
    }
}
