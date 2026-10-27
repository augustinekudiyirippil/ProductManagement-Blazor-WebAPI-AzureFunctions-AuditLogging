using ProductManagement.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace ProductManagement.Api.Repositories
{
    public class ProductImageRepository
     : IProductImageRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductImageRepository(
            ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(
            ProductImage image)
        {
            _context.ProductImages.Add(image);

            await _context.SaveChangesAsync();
        }


        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<List<ProductImage>>GetByProductIdAsync(int productId)
        {
            return await _context.ProductImages
                .Where(x =>
                    x.ProductId == productId &&
                    !x.IsDeleted)
                .ToListAsync();
        }



        public async Task<ProductImage?>GetByIdAsync(int imageId)
        {
            return await _context.ProductImages
                .FirstOrDefaultAsync(x =>
                    x.Id == imageId &&
                    !x.IsDeleted);
        }
    }


}
