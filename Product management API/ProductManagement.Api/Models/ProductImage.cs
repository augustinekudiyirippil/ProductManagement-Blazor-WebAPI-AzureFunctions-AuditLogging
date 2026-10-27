using ProductManagement.Api.Models;

public class ProductImage
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public string FileName { get; set; } = string.Empty;

    public string BlobUrl { get; set; } = string.Empty;

    public DateTime UploadedDate { get; set; }

    public bool IsPrimary { get; set; }

    public bool IsDeleted { get; set; }
    public Product Product { get; set; } = null!;
}