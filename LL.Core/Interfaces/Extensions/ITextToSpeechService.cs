namespace LL.Core.Interfaces.Extensions;

public interface ITextToSpeechService
{
    void CreateAudio(string text, int langId);
}