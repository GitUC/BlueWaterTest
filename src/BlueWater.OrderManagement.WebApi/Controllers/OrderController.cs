using BlueWater.OrderManagement.Common.Contracts;
using BlueWater.OrderManagement.Services.Interfaces;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using System;
using Swashbuckle.AspNetCore.Annotations;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BlueWater.OrderManagement.OrderMangement.WebApi.Controllers
{
    [Route("api/order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private IOrderProcess _orderProcess;
        private readonly IBackgroundJobClient _backgroundJobClient;
        private ILogger<OrderController> _logger;

        public OrderController(IOrderProcess orderProcess, IBackgroundJobClient backgroundJobClient, ILogger<OrderController> logger)
        {
            _orderProcess = orderProcess ?? throw new ArgumentNullException(nameof(orderProcess));

            _backgroundJobClient = backgroundJobClient ?? throw new ArgumentNullException(nameof(backgroundJobClient));

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        }


        /// <summary>
        /// Get order job's status
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getOrderStatus/{jobId}")]
        public IActionResult GetOrderStatusById([BindRequired] string jobId)
        {
            try
            {
                var status = _orderProcess.GetOrderStatus(jobId);

                return string.IsNullOrEmpty(status) ? NotFound() : Ok(status);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get order status by Id: {jobId}, details {ex.Message}");

                return BadRequest();
            }
        }

        /// <summary>
        /// The operation supports to create a order process in the system 
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("createOrder")]
        [SwaggerOperation(Tags = new[] { "Order Payload" })]
        public IActionResult CreateOrderProcess([FromBody] Orders order)
        {

            var jobId = _backgroundJobClient.Enqueue(() => _orderProcess.CreateOrder(order));

            _logger.LogInformation($"Start create order job and job id is {jobId}.");
            return Ok(jobId);
        }


        /// <summary>
        /// The operation supports to start the scheduled  Despatch orders job at the given datetime or delayed time
        /// </summary>
        /// <param name="scheduleDateTime"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("scheduleJob")]
        [SwaggerOperation(Tags = new[] { "Task Scheduled time" })]
        public IActionResult DispathOrderProcess(ScheduleDateTime scheduleDateTime)
        {
            var timeSpan = new TimeSpan(scheduleDateTime.Hours, scheduleDateTime.Minutes, scheduleDateTime.Seconds);

            var jobId = string.Empty;

            if (DateTime.TryParse(scheduleDateTime.ScheduleTime, out DateTime jobStartTime))
            {
                var startDateTimeOffset = new DateTimeOffset(jobStartTime);
                jobId = _backgroundJobClient.Schedule(() => _orderProcess.DispatchOrder(), startDateTimeOffset);
            }
            else if (timeSpan.TotalMinutes > 0)
            {
                jobId = _backgroundJobClient.Schedule(() => _orderProcess.DispatchOrder(), timeSpan);
            }
            else
            {
                return BadRequest("The given sheduled datetime is not valid.");
            }

            _logger.LogInformation($"Start dispatch orders job and job id is {jobId}.");
            return Ok(jobId);
        }

    }
}
