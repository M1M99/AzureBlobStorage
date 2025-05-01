using Azure.Storage.Blobs;
using AzureBlob.Services;

public class BlobStorageService:IBlobStorageService
{
    private readonly string _connectionString;
    public BlobStorageService(IConfiguration configuration)
    {
        _connectionString = configuration["AzureConnectionString:ConnectionStrings"];
    }

    private BlobContainerClient GetContainerClient(string containerName)
    {
        return new BlobContainerClient(_connectionString, containerName);
    }

    public async Task<string> UploadAsync(Stream fileStream, string fileName, string containerName)
    {
        var containerClient = GetContainerClient(containerName);
        await containerClient.CreateIfNotExistsAsync();
        var blobClient = containerClient.GetBlobClient(fileName);
        await blobClient.UploadAsync(fileStream, overwrite: true);
        return blobClient.Uri.ToString();
    }

    public async Task<Stream> DownloadAsync(string fileName, string containerName)
    {
        var containerClient = GetContainerClient(containerName);
        var blobClient = containerClient.GetBlobClient(fileName);
        var download = await blobClient.DownloadAsync();
        return download.Value.Content;
    }

    public async Task<List<string>> ListFilesAsync(string containerName)
    {
        var containerClient = GetContainerClient(containerName);
        var blobUris = new List<string>();
        await foreach (var blobItem in containerClient.GetBlobsAsync())
        {
            var blobClient = containerClient.GetBlobClient(blobItem.Name);
            blobUris.Add(blobClient.Uri.ToString());
        }
        return blobUris;
    }
}
