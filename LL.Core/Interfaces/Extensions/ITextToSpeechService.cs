using LL.Core.Enums;

namespace LL.Core.Interfaces.Extensions;

public interface ITextToSpeechService
{
    byte[] CreateAudio(string text, LanguageEnum lang);
}