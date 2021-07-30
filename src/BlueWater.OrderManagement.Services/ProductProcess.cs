using BlueWater.OrderManagement.Common.Contracts;
using BlueWater.OrderManagement.Services.Interfaces;
using BlueWater.OrderMangement.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueWater.OrderMangement.DataAccess.DataModel;
using Microsoft.Extensions.Logging;

namespace BlueWater.OrderManagement.Services
{
    public class ProductProcess : IProductProcess
    {
        private IProductDataProcess _dataProcessor;
        private ILogger<ProductProcess> _logger;

        public ProductProcess(IProductDataProcess dataProcessor, ILogger<ProductProcess> logger)
        {
            _dataProcessor = dataProcessor ?? throw new ArgumentNullException(nameof(dataProcessor));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Guid> CreateProductAsync(ProductPayload productDto)
        {
            var productId = Guid.NewGuid();

            Product product = new()
            {
                ProductId = productId,
                ProductName = productDto.ProductName,
                UnitPrice = productDto.UnitPrice,
                LastEnditTime = DateTime.UtcNow
            };

            await _dataProcessor.CreateProductAsync(product);
            _logger.LogInformation($"Completed creating a new product for {product.ProductName}.");

            return productId;
        }

        public async Task<bool> DeleteProductByIdAsync(Guid productId)
        {
            var result = await _dataProcessor.DeleteProductByIdAsync(productId);

            _logger.LogInformation($"Completed delete product {productId}, result: {result}");

            return result;
        }

        public async Task<ProductDto> GetProductByIdAsync(Guid productId)
        {
            var product = await _dataProcessor.GetProductAsync(productId);

            if (product != null)
            {
                return new ProductDto
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    UnitPrice = product.UnitPrice
                };
            }
            else
            {
                _logger.LogInformation($"Product wth {productId} not existed in the system" );
                return null;
            }
        }

        public async Task<IEnumerable<ProductDto>> GetProducts()
        {
            var result = await _dataProcessor.GetProductsAsync();
            
            return result.Select(t => new ProductDto {ProductId=t.ProductId, ProductName= t.ProductName, UnitPrice= t.UnitPrice });
        }

        public async Task<bool> UpdateProductUnitPriceAsync(Guid productId, double price)
        {
            var result = await _dataProcessor.UpdateProductAsync(productId, price);
            _logger.LogInformation($"Completed delete product {productId}, result: {result}");

            return result;
        }
    }
}
