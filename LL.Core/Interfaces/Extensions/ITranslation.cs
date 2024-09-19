namespace LL.Core.Interfaces.Extensions;

public interface ITranslation
{
    string TranslateText(string text, int fromId, int toId);
}