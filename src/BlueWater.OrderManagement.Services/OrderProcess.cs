using BlueWater.OrderManagement.Common.Contracts;
using BlueWater.OrderManagement.Services.Interfaces;
using Hangfire;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace BlueWater.OrderManagement.Services
{
    public class OrderProcess : IOrderProcess
    {
        private readonly ILogger<OrderProcess> _logger;

        public OrderProcess(ILogger<OrderProcess> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
         
        }

        public async Task CreateOrder(Orders order)
        {
            try
            {
                // to background task, long running
                await Task.Delay(20000);
                _logger.LogInformation("Order job has been craeted in the system.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed in CreateOrder process, message: {ex.Message}.", ex);
                throw;
            }
        }

        public async Task DispatchOrder()
        {
            try
            {
                // to background task, long running
                await Task.Delay(5000);

                _logger.LogInformation("Dispatch job has completed in the system.");
            }
            catch(Exception ex)
            {
                _logger.LogError($"Failed in DispatchOrder process, message: {ex.Message}.", ex);
                throw;
            }
        }

        public string GetOrderStatus(string jobId)
        {
            var connection = JobStorage.Current.GetConnection();
            var jobData = connection.GetJobData(jobId);

            return jobData?.State;
        }
    }
}
