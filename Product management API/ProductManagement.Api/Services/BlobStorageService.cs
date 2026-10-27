using Azure.Storage.Blobs;
using Microsoft.Extensions.Options;
using ProductManagement.Api.Configuration;
using ProductManagement.Api.Services;
using Azure.Storage.Sas;


public class BlobStorageService
    : IBlobStorageService
{
    private readonly BlobStorageSettings _settings;

    public BlobStorageService(
        IOptions<BlobStorageSettings> options)
    {
        _settings = options.Value;
    }

    public async Task<string> UploadAsync(
        int productId,
        IFormFile file)
    {
        var containerClient =
            new BlobContainerClient(
                _settings.ConnectionString,
                _settings.ContainerName);

        var blobName =
            $"{productId}/{Guid.NewGuid()}-{file.FileName}";

        var blobClient =
            containerClient.GetBlobClient(blobName);

        using var stream =
            file.OpenReadStream();

        await blobClient.UploadAsync(
            stream,
            overwrite: true);

        return blobClient.Uri.ToString();
    }


    public string GenerateReadSasUrl(
    string blobUrl)
    {
        var containerClient =
            new BlobContainerClient(
                _settings.ConnectionString,
                _settings.ContainerName);

        var blobUri =
            new Uri(blobUrl);

        var segments =
            blobUri.AbsolutePath.Split(
                '/',
                StringSplitOptions.RemoveEmptyEntries);

        //var blobName =
        //    string.Join("/",
        //        segments.Skip(1));

        var blobName =
             Uri.UnescapeDataString(
             string.Join("/",
             segments.Skip(1)));

        var blobClient =
            containerClient.GetBlobClient(
                blobName);

        var sasBuilder =
            new BlobSasBuilder
            {
                BlobContainerName =
                    _settings.ContainerName,

                BlobName =
                    blobName,

                Resource = "b",

                ExpiresOn =
                    DateTimeOffset.UtcNow
                        .AddHours(1)
            };

        sasBuilder.SetPermissions(
            BlobSasPermissions.Read);

        return blobClient
            .GenerateSasUri(
                sasBuilder)
            .ToString();
    }



}
