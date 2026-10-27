using ProductManagement.Api.Models;

namespace ProductManagement.Api.Repositories;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllAsync();

    Task<Product?> GetByIdAsync(int id);

    Task<Product> AddAsync(Product product);

    Task UpdateAsync(Product product);

    Task DeleteAsync(Product product);
}