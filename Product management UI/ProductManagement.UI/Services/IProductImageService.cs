using Microsoft.AspNetCore.Components.Forms;
using ProductManagement.UI.DTOs;

namespace ProductManagement.UI.Services;

public interface IProductImageService
{
    Task UploadImageAsync(
        int productId,
        IBrowserFile file,
        bool isPrimary);


    Task<List<ProductImageDto>>
        GetImagesAsync(int productId);

    Task DeleteImageAsync(int imageId);

}