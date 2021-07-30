using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using Serilog;
using System.Reflection;
using System.IO;
using BlueWater.OrderManagement.Services;
using BlueWater.OrderManagement.Services.Interfaces;
using BlueWater.OrderMangement.DataAccess;
using BlueWater.OrderManagement.Common.Contracts;
using Microsoft.Extensions.Logging;

namespace BlueWater.OrderManagement.WebApi
{
    public class Startup
    {

        private readonly string SqlDatabaseConnection = "SqlDatabaseConnection";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            /*
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddMicrosoftIdentityWebApi(Configuration);

            var roles = new List<string>() { "DaemonAppRole" };


            services.AddAuthorization(options =>
                options.AddPolicy("DaemonAppRolePolicy",
                policy => policy.RequireRole(roles)));

            */

            services.AddControllers();

            services.AddServiceModule();
            services.AddDataModule(Configuration, SqlDatabaseConnection, false);

            /* 
            // memory storage for hangfire
            services.AddHangfire(config => config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                                           .UseSimpleAssemblyNameTypeSerializer()
                                           .UseDefaultTypeSerializer()
                                           .UseMemoryStorage());
            */


            services.AddHangfire(configuration => configuration
                  .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                  .UseSimpleAssemblyNameTypeSerializer()
                  .UseRecommendedSerializerSettings()
                  .UseSqlServerStorage(Configuration.GetConnectionString(SqlDatabaseConnection), new SqlServerStorageOptions
                  {
                      CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                      SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                      QueuePollInterval = TimeSpan.Zero,
                      UseRecommendedIsolationLevel = true,
                      DisableGlobalLocks = true
                  }));


            services.AddHangfireServer();

            ConfigureSwagger(services);
        }

        private static void ConfigureSwagger(IServiceCollection services)
        {
            // Register the Swagger generator, and set UI
            services.AddSwaggerGen(
                c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Version = "v1",
                        Title = "BlueWater Control Task API",
                        Description = "A simple example ASP.NET Core Web API",
                        TermsOfService = new Uri("https://blueWatercontrol.com/terms"),
                        Contact = new OpenApiContact
                        {
                            Name = "Support Team",
                            Email = string.Empty,
                            Url = new Uri("https://blueWatercontrol.com/support"),
                        },
                        License = new OpenApiLicense
                        {
                            Name = "Use under License",
                            Url = new Uri("https://blueWatercontrol.com/license"),
                        }
                    });

                    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                    c.IncludeXmlComments(xmlPath);
                
                    var securitySchema = new OpenApiSecurityScheme
                    {
                        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.Http,
                        Scheme = "bearer",
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    };

                    c.AddSecurityDefinition("Bearer", securitySchema);

                    var securityRequirement = new OpenApiSecurityRequirement
                    {
                        { securitySchema, new[] { "Bearer" } }
                    };

                    c.AddSecurityRequirement(securityRequirement);
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app, IWebHostEnvironment env, IBackgroundJobClient backgroundJobClient,
            IRecurringJobManager recurringJobManager, IServiceProvider serviceProvider, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseSerilogRequestLogging();

            app.UseHttpsRedirection();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Web API V1");
            });

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHangfireDashboard();
            });

            ExecuteScheduleJob(backgroundJobClient, recurringJobManager, serviceProvider, logger);

        }


        // Based on the setting in appsettings, schedule background task running
        private void ExecuteScheduleJob(IBackgroundJobClient backgroundJobClient, IRecurringJobManager recurringJobManager, IServiceProvider serviceProvider, ILogger<Startup> logger)
        {
            try
            {
                var taskSettings = Configuration.GetSection("backendScheduleDateTime")?.Get<BackendScheduleDateTime>();

                if (taskSettings != null)
                {
                    if (taskSettings.JobType == ScheduleType.OneOnly)
                    {
                        var timeDelay = new TimeSpan(taskSettings.ScheduleDateTime.Hours, taskSettings.ScheduleDateTime.Minutes, taskSettings.ScheduleDateTime.Seconds);
                        var minDelay = new TimeSpan(0, 0, 1);

                        if (timeDelay > minDelay)
                        {
                            backgroundJobClient.Schedule(() => serviceProvider.GetService<IOrderProcess>().DispatchOrder(), timeDelay);
                        }
                        else
                        {
                            logger.LogWarning("'backendScheduleDateTime' setting is invalid, delay time cannot be zero or negative value.");
                        }
                    }
                    else if (taskSettings.JobType == ScheduleType.Recurring)
                    {
                        var cronExpress = string.Concat("**", taskSettings.ScheduleDateTime.Hours > 0 ? taskSettings.ScheduleDateTime.Hours : "*",
                                                            taskSettings.ScheduleDateTime.Minutes > 0 ? taskSettings.ScheduleDateTime.Minutes : "*",
                                                            taskSettings.ScheduleDateTime.Seconds > 0 ? taskSettings.ScheduleDateTime.Seconds : "*");

                        logger.LogInformation($"Recurring job is starting and CRON setting is {cronExpress}");

                        recurringJobManager.AddOrUpdate(
                                    "Run Recurring Job",
                                    () => serviceProvider.GetService<IOrderProcess>().DispatchOrder(),
                                    cronExpress);
                    }
                    else
                    {
                        logger.LogWarning($"Failed to execute schedule task, invalid job type.");
                    }
                }
                else
                {
                    logger.LogWarning("'backendScheduleDateTime' setting is invalid.");
                }
            }
            catch(Exception  ex)
            {
                logger.LogDebug($"Failed to excute scheduled job, StackTrace {ex.StackTrace}.");
                logger.LogError($"Failed to excute scheduled job, reason: {ex.Message}");
            }
        }
    }
}
