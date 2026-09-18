using LL.Core.Interfaces.Extensions;
using LL.Resources.Contexts;
using Microsoft.Extensions.DependencyInjection;

namespace LL.Test.Services;

public class AppMonitoringServiceTest
{
    private readonly IAppMonitoringService _appMonitoringService;
    private readonly AppDbContext _db;

    public AppMonitoringServiceTest()
    {
        var serviceProvider = Provider.GetRequiredService().BuildServiceProvider();
        _appMonitoringService = serviceProvider.GetRequiredService<IAppMonitoringService>();
        _db = serviceProvider.GetRequiredService<AppDbContext>();
    }

    [Fact]
    public void ExportError_ShouldPersistServerError()
    {
        var before = _db.ServerErrors.Count();

        _appMonitoringService.ExportError(
            new InvalidOperationException("unit-test-error"),
            new Dictionary<string, object> { ["source"] = "AppMonitoringServiceTest" });

        Assert.True(_db.ServerErrors.Count() > before);
        Assert.Contains(_db.ServerErrors, e => e.InnerException.Contains("unit-test-error"));
    }

    [Fact]
    public void ExportError_WithoutData_ShouldPersistServerError()
    {
        var before = _db.ServerErrors.Count();

        _appMonitoringService.ExportError(new Exception("plain-error"));

        Assert.True(_db.ServerErrors.Count() > before);
    }
}
