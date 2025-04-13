using LL.Core.Interfaces.Extensions;
using System.Net;
using LL.Core.Helpers;
using LL.Resources.Contexts;
using LL.Resources.Models;

namespace LL.Resources.Services;

public class AppMonitoringService(AppDbContext db) : IAppMonitoringService
{
    public void ExportError(Exception exception)
    {
        ExportError(exception, new Dictionary<string, object>());
    }

    public void ExportError(Exception exception, Dictionary<string, object> ExceptionData)
    {
        string hostName = Dns.GetHostName();
        IPHostEntry entry = Dns.GetHostEntry(hostName);

        var serverError = new ServerError
        {
            InnerException = exception.InnerException?.Message ?? exception.Message,
            StackTrace = exception.StackTrace ?? string.Empty,
            InternetProtocol = entry.ToString() ?? string.Empty,
            Data = JsonHelper.SerializeObject(ExceptionData),
            CreatedDate = DateTime.Now,
        };
        
        db.Add(serverError);
        db.SaveChanges();
    }
}

