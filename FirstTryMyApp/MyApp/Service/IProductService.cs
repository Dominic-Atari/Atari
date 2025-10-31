using MyApp.Dtos;
//using MyApp.Models;

namespace MyApp.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(int id);
        Task<Product> CreateAsync(ProductDto dto);
        Task<Product?> UpdateAsync(int id, ProductDto dto);
        Task<bool> DeleteAsync(int id);
    }
}