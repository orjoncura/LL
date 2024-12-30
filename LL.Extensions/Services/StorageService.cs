using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using LL.Core.Interfaces.Extensions;
using LL.Core.Models.DataTransferObjects;

namespace LL.Extensions.Services;

public class StorageService : IStorageService
{
    public async Task<string> SaveFile(StorageModel model, byte[] file)
    {
        if(file.Length == 0)
            return string.Empty;
        
        // Create S3 client
        var s3Client = new AmazonS3Client(model.AccessKey, model.SecretKey, RegionEndpoint.EUWest2);
        
        // Use TransferUtility for uploading
        string keyName = Guid.NewGuid().ToString();
        var fileTransferUtility = new TransferUtility(s3Client);
        await fileTransferUtility.UploadAsync(new MemoryStream(file), model.BucketName, keyName);

        return $"https://{model.BucketName}.s3.eu-west-2.amazonaws.com/{keyName}";
    }
    
    public async Task<byte[]> GetFile(StorageModel model, string filePath)
    {
        // Create S3 client
        var s3Client = new AmazonS3Client(model.AccessKey, model.SecretKey, RegionEndpoint.EUWest2);
        
        // Initialize TransferUtility for file download
        var fileTransferUtility = new TransferUtility(s3Client);

        // Specify a temporary path to save the file
        string tempFilePath = Path.GetTempFileName();

        // Download the file from S3
        await fileTransferUtility
            .DownloadAsync(tempFilePath, model.BucketName, filePath.Replace($"https://{model.BucketName}.s3.eu-west-2.amazonaws.com/", ""));
        
        // Read the downloaded file as a byte array
        byte[] fileBytes = await File.ReadAllBytesAsync(tempFilePath);

        // Cleanup
        File.Delete(tempFilePath);

        return fileBytes; // Return file content as byte array
    }
}