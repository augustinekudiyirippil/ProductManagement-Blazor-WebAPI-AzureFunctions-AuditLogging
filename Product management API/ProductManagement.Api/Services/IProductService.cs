using ProductManagement.Api.Models;

public interface IProductService
{
    Task<IEnumerable<Product>> GetAllProductsAsync();

    Task<Product?> GetProductByIdAsync(int id);

    Task<Product> CreateProductAsync(Product product,int userId);

    Task UpdateProductAsync(  Product product,  int userId);

    Task DeleteProductAsync(
    int id,
    int userId,
    string productName);

    Task<bool> MakeUnavailableAsync(
    int productId,
    int userId,
    string productName);

    Task<bool> MakeAvailableAsync(
    int productId,
    int userId,
    string productName);


}