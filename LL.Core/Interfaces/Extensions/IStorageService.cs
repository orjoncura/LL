using LL.Core.Models.DataTransferObjects;

namespace LL.Core.Interfaces.Extensions;

public interface IStorageService
{
    Task<string> SaveFile(StorageModel model, byte[] file);
    Task<byte[]> GetFile(StorageModel model, string filePath);
}