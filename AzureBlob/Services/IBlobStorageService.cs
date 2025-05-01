namespace AzureBlob.Services
{
    public interface IBlobStorageService
    {
        Task<string> UploadAsync(Stream fileStream, string fileName, string containerName); // ihave2continer(img,video)
        Task<Stream> DownloadAsync(string fileName, string containerName);
        Task<List<string>> ListFilesAsync(string containerName);
    }
}
