namespace LL.Core.Interfaces.Extensions;

public interface ITranslationService
{
    Task<string> TranslateText(string text, int fromId, int toId);
}