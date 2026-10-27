namespace ProductManagement.Api.Configuration;

public class BlobStorageSettings
{
    public string ConnectionString { get; set; }
        = string.Empty;

    public string ContainerName { get; set; }
        = string.Empty;
}