namespace LL.Core.Interfaces.Extensions;

public interface IAppMonitoringService
{
    void ExportError(Exception exception);

    void ExportError(Exception exception, Dictionary<string, object> ExceptionData);
}