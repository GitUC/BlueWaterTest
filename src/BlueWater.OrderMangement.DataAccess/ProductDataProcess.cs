using BlueWater.OrderMangement.DataAccess.DataModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlueWater.OrderMangement.DataAccess
{
    public class ProductDataProcess : IProductDataProcess
    {
        private OrderDataContext _orderDataContext;
        public ProductDataProcess(OrderDataContext orderDataContext)
        {
            _orderDataContext = orderDataContext ?? throw new ArgumentNullException(nameof(orderDataContext));
        }

        public async Task CreateProductAsync(Product product)
        {
            await _orderDataContext.Products.AddAsync(product);
            await _orderDataContext.SaveChangesAsync();
        }


        public async Task<Product> GetProductAsync(Guid productId)
        {
            return await _orderDataContext.Products.Where(t => t.ProductId == productId).SingleOrDefaultAsync<Product>();
        }

        public async Task<bool> UpdateProductAsync(Guid productId, double unitPrice)
        {
            bool result = false;
            var product = _orderDataContext.Products.Where(t => t.ProductId == productId).SingleOrDefault<Product>();

            if (product != null)
            {
                product.UnitPrice = unitPrice;
                await _orderDataContext.SaveChangesAsync();
                result = true;
            }

            return result;
        }

        public async Task<bool> DeleteProductByIdAsync(Guid porductId)
        {
            bool result = false;
            var product = _orderDataContext.Products.Where(t => t.ProductId == porductId).SingleOrDefault<Product>();

            if (product != null)
            {
                _orderDataContext.Remove(product);
                await _orderDataContext.SaveChangesAsync();
                result = true;
            }

            return result;
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            return await _orderDataContext.Products.ToListAsync();
        }
    }
}
