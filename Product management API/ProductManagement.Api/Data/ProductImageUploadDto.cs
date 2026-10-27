namespace ProductManagement.Api.DTOs;

public class ProductImageUploadDto
{
    public IFormFile File { get; set; } = null!;

    public bool IsPrimary { get; set; }
}