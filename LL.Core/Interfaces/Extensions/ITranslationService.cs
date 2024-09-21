namespace LL.Core.Interfaces.Extensions;

public interface ITranslationService
{
    string TranslateText(string text, int fromId, int toId);
}