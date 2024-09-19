using LL.Core.Interfaces.Extensions;

namespace LL.Extensions;

public class AppMonitoring : IAppMonitoring
{
    public void ExportError(Exception exception)
    {
        ExportError(exception, new Dictionary<string, object>());
    }

    public void ExportError(Exception exception, Dictionary<string, object> ExceptionData)
    {
    }
}

