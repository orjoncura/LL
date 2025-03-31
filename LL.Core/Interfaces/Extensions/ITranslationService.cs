namespace LL.Core.Interfaces.Extensions;

public interface ITranslationService
{
    Task<List<string>> TranslateText(string text, int fromId, int toId);
}