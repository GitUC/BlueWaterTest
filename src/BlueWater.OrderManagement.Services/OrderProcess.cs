using BlueWater.OrderManagement.Common.Contracts;
using BlueWater.OrderManagement.Services.Interfaces;
using Hangfire;
using Microsoft.Extensions.Logging;
using System;

namespace BlueWater.OrderManagement.Services
{
    public class OrderProcess : IOrderProcess
    {
        private readonly ILogger<OrderProcess> _logger;

        public OrderProcess(ILogger<OrderProcess> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
         
        }

        public void CreateOrder(Orders order)
        {
            try
            {
                // to background task, long running
                System.Threading.Thread.Sleep(20000);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed in CreateOrder process, message: {ex.Message}.", ex);
                throw;
            }
        }

        public void DispatchOrder()
        {
            try
            {
                // to background task, long running
                System.Threading.Thread.Sleep(5000);
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
