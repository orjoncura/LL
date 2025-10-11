namespace LL.Core.Interfaces.Extensions;

public interface ITranscriptionService
{ 
    Task<string> TranscribeFromUrl(string videoUrl);
}