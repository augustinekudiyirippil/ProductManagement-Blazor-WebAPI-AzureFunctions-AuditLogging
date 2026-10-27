namespace ProductManagement.Api.Repositories
{
    public interface IProductImageRepository
    {
        Task AddAsync(ProductImage image);

       
        Task<List<ProductImage>> GetByProductIdAsync(
            int productId);

        Task<ProductImage?> GetByIdAsync(int imageId);

        Task SaveChangesAsync();
    }
}
