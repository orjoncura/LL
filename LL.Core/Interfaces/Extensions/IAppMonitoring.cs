namespace LL.Core.Interfaces.Extensions;

public interface IAppMonitoring
{
    void ExportError(Exception exception);

    void ExportError(Exception exception, Dictionary<string, object> ExceptionData);
}