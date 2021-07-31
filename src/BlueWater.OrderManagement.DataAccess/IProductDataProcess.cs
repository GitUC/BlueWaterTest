using BlueWater.OrderMangement.DataAccess.DataModel;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BlueWater.OrderMangement.DataAccess
{
    public interface IProductDataProcess
    {
        Task CreateProductAsync(Product product);

        Task<bool> DeleteProductByIdAsync(Guid porductId);

        Task<Product> GetProductAsync(Guid productId);

        Task<bool> UpdateProductAsync(Guid productId, double unitPrice);

        Task<IEnumerable<Product>> GetProductsAsync();
    }
}