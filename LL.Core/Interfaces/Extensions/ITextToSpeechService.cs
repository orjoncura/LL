namespace LL.Core.Interfaces.Extensions;

public interface ITextToSpeechService
{
    byte[] CreateAudio(string text, int langId);
}