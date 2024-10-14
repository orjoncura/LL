using LL.Core.Constants;
using LL.Core.Interfaces.Services;
using Microsoft.Extensions.Configuration;

namespace LL.Test.Services;

public class SecurityServiceTest
{
    private ISecurityService _securityService { get; set; }
    private IConfiguration _config { get; set; }
    
    public SecurityServiceTest()
    {
        _securityService = Provider.GetRequiredService<ISecurityService>();
        _config = Provider.GetConfiguration<ISecurityService>();
    }
        
    [Fact]
    public async Task CreateSeminar_ShouldCreateSeminar()
    {     
        bool isRequestCreated = false;
        
        string email = "test@test.com";
        var ipAddress = string.Empty;
        
        var attemptsLimit = Convert.ToInt32(_config[Secrets.AttemptsLimit]);
        
        isRequestCreated = _securityService.CreateNewUserRequest(email, ipAddress, attemptsLimit);
        
        // Assert
        Assert.True(isRequestCreated);
    } 
}