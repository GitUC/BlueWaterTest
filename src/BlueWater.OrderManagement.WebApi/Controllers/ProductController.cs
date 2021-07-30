using Microsoft.AspNetCore.Mvc;
using BlueWater.OrderManagement.Common.Contracts;
using BlueWater.OrderManagement.Services.Interfaces;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BlueWater.OrderManagement.WebApi.Controllers
{
    [Route("api/product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private IProductProcess _productPorcessor;
        private ILogger<ProductController> _logger;
        public ProductController(IProductProcess productPorcessor, ILogger<ProductController> logger)
        {
            _productPorcessor = productPorcessor ?? throw new ArgumentNullException(nameof(productPorcessor));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Get all products in the system
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var products = await _productPorcessor.GetProducts();

            return Ok(products);
        }


        /// <summary>
        /// Get porduct by productId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPruductById(Guid id)
        {
            var product = await _productPorcessor.GetProductByIdAsync(id);
            if(product != null)
            {
                return Ok(product);
            }
            else
            {
                return NotFound($"Product '{id}' not existed in the system.");
            }
        }

        /// <summary>
        /// Create a new product
        /// </summary>
        /// <param name="productPayload"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(201)]
        public async Task<ActionResult> CreateProduct([FromBody] ProductPayload productPayload)
        {
           var productId = await _productPorcessor.CreateProductAsync(productPayload);

            var product = new ProductDto
            {
                ProductId = productId,
                ProductName = productPayload.ProductName,
                UnitPrice = productPayload.UnitPrice
            };

            return new ObjectResult(product) { StatusCode = 201 };
        }

        /// <summary>
        /// update a product
        /// </summary>
        /// <param name="id"></param>
        /// <param name="productUnitPrice"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(Guid id, double productUnitPrice)
        {
            var result = await _productPorcessor.UpdateProductUnitPriceAsync(id, productUnitPrice);

            return result ? Ok() : NotFound($"Product '{id}' not existed in the system.");
        }

        /// <summary>
        /// Delete a product by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var result = await _productPorcessor.DeleteProductByIdAsync(id);

            return result ? Ok() : NotFound($"Product '{id}' not existed in the system.");
        }
    }
}
