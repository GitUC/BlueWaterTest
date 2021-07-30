using BlueWater.OrderManagement.Common.Contracts;
using BlueWater.OrderManagement.Services.Interfaces;
using Hangfire;
using Microsoft.Extensions.Logging;
using System;

namespace BlueWater.OrderManagement.Services
{
    public class OrderProcess : IOrderProcess
    {
        //private readonly ILogger<OrderProcess> _logger;

        public OrderProcess(ILogger<OrderProcess> logger)
        {
            /*
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            _logger = logger;
            */
        }

        public Guid CreateOrder(Orders order)
        {
            // to background task, long running
            var result = Guid.NewGuid();

            System.Threading.Thread.Sleep(20000);

            return result;
        }

        public void DispatchOrder()
        {
            // to background task, long running
            System.Threading.Thread.Sleep(5000);
        }

        public string GetOrderStatus(string jobId)
        {
            var connection = JobStorage.Current.GetConnection();
            var jobData = connection.GetJobData(jobId);

            return jobData?.State;
        }
    }
}
