namespace LL.Core.Models.DataTransferObjects;

public class StorageModel
{
    public string AccessKey { get; set; }
    
    public string SecretKey { get; set; } 
    
    public string BucketName { get; set; }
    
    public StorageModel(string accessKey, string secretKey, string bucketName)
    {
        AccessKey = accessKey;
        SecretKey = secretKey;
        BucketName = bucketName;
    }
}