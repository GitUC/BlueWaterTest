using BlueWater.OrderManagement.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlueWater.OrderManagement.Services.Interfaces
{
    public interface IProductProcess
    {
        Task<Guid> CreateProductAsync (ProductPayload porduct);

        Task<ProductDto> GetProductByIdAsync(Guid productId);

        Task<bool> DeleteProductByIdAsync(Guid productId);

        Task<bool> UpdateProductUnitPriceAsync(Guid productId, double price);

        Task<IEnumerable<ProductDto>> GetProducts();
    }
}
