using ValueObjects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services
{
    public interface IProductsService
    {
        Task<VOProduct> GetProductByIdAsync(int productId);
        Task<IEnumerable<VOProduct>> GetAllProductsAsync();
        Task<int> CreateProductAsync(VOProduct product);
        Task<bool> UpdateProductAsync(int productId, VOProduct product);
        Task<bool> DeleteProductAsync(int productId);
    }
}
