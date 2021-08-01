using BlueWater.OrderManagement.Common.Contracts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Serilog;
using System.Net.Http;
using TechTalk.SpecFlow;

namespace BlueWater.OrderManagement.Test.Hooks
{
    [Binding]
    public static class HostingContext
    {
        internal static IConfiguration Configuration { get; set; }

        /// <summary>
        /// Gets microsoft Test Server
        /// </summary>
        internal static TestServer Server { get; private set; }

        /// <summary>
        /// payload for Produt
        /// </summary>
        internal static ProductPayload ProductPayload { get; set; } 

        /// <summary>
        /// Product item
        /// </summary>
        internal static ProductDto Product { get; set; }

        internal static Orders OrderPayload { get; set; }

        internal static ScheduleDateTime ScheduleDateTime { get; set; }

        internal static string JobId { get; set; }

        /// <summary>
        /// Initial Setup Before the component tests run
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
           
        }
    }
}
