using LL.Core.Constants;
using LL.Core.Interfaces.Services;
using LL.Core.Models.Arguments;
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
    public async Task CreateSeminar_ShouldCreateNewUserRequest()
    {     
        bool isRequestCreated = false;

        var model = new NewUserModel();
        model.Email = "test@test.com";
        model.ConfirmEmail = "test@test.com";
        
        var attemptsLimit = Convert.ToInt32(_config[Secrets.AttemptsLimit]);
        
        isRequestCreated = _securityService.CreateNewUserRequest(model, string.Empty, attemptsLimit);
        
        // Assert
        Assert.True(isRequestCreated);
    } 
}