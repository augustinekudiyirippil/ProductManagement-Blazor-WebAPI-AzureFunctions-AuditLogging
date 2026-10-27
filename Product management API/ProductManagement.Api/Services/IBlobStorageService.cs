namespace ProductManagement.Api.Services
{
    public interface IBlobStorageService
    {
        Task<string> UploadAsync(
            int productId,
            IFormFile file);

        string GenerateReadSasUrl(
            string blobUrl);
    }
}
