using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueWater.OrderManagement.WebApi.Controllers
{
    public class ErrorController : ControllerBase
    {
        private ILogger<ErrorController> _logger;
        
        public ErrorController(ILogger<ErrorController> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [Route("/error-development")]
        public IActionResult ErrorLocalDevelopment(
            [FromServices] IWebHostEnvironment webHostEnvironment)
        {
            if (webHostEnvironment.EnvironmentName != "Development")
            {
                throw new InvalidOperationException(
                    "This shouldn't be invoked in non-development environments.");
            }

            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();

            _logger.LogError(context.Error.Message, context.Error.StackTrace);

            return Problem(
                detail: context.Error.StackTrace,
                title: context.Error.Message);
        }

        [HttpGet]
        [Route("/error")]
        public IActionResult Error()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            _logger.LogError(context.Error.Message, context.Error.StackTrace);

            return Problem(
                title: context.Error.Message);
        }
    }
}
