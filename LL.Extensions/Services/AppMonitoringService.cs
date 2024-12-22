using LL.Core.Interfaces.Extensions;

namespace LL.Extensions;

public class AppMonitoringService : IAppMonitoringService
{
    public void ExportError(Exception exception)
    {
        ExportError(exception, new Dictionary<string, object>());
    }

    public void ExportError(Exception exception, Dictionary<string, object> ExceptionData)
    {
    }
}

