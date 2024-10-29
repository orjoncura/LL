using LL.Core.Constants;
using LL.Core.Interfaces.Services;
using LL.Core.Models.Arguments;
using LL.Core.Services;
using LL.Data.Contexts;
using LL.Data.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LL.Test.Services;

public class SecurityServiceTest
{
    private ISecurityService _securityService { get; set; }
    private IConfiguration _config { get; set; }
    
    private string Token { get; set; }
    public SecurityServiceTest()
    {
        _securityService = Provider.GetRequiredService<ISecurityService>();
        _config = Provider.GetConfiguration<ISecurityService>();
        
        using (var context = Provider.GetRequiredService<AppDBContext>())
        {
            // Ensure the database is created
            context.Database.EnsureCreated();

            // Add data to the context
            context.Users.Add(new User()
            {
                Id = 1,
                Email = "admin@admin.com",
                PasswordHash = "password",
                IsActive = true
            });

            // Save changes to the database
            context.SaveChanges();
        }
        
        Token = Guid.NewGuid().ToString();
    }
    
    [Fact]
    public async Task RegisterUser_ShouldRegisterNewUserRequest()
    {     
        bool isRequestCreated = false;

        var model = new NewUserModel();
        model.Email = "test@test.com";
        model.ConfirmEmail = "test@test.com";
        
        string ip = "123.123.123.123";
        string url = "http://localhost:3000";
        
        isRequestCreated = _securityService.RegisterUser(model, ip, url);
        
        // Assert
        Assert.True(isRequestCreated);
    } 
    [Fact]
    public async Task CompleteUserRegistration_ShouldRegisterNewUser()
    {     
        bool isRequestCompleted = false;

        var model = new ConfirmationModel();
        model.Password = "Password";
        model.ConfirmPassword = "Password";
        model.Token = Token;
        
        isRequestCompleted = _securityService.CompleteUserRegistration(model);
        
        // Assert
        Assert.True(isRequestCompleted);
    } 
    [Fact]
    public async Task ResetPassword_ShouldCreateNewResetPasswordRequest()
    {     
        bool isRequestCreated = false;
        string email = "test@test.com";
        string ip = "123.123.123.123";
        
        int attemptsLimit = 3;
        string url = "http://localhost:3000";
        
        isRequestCreated = _securityService.ResetPassword(email, ip, url);
        
        // Assert
        Assert.True(isRequestCreated);
    } 
    [Fact]
    public async Task CompletePasswordReset_ShouldCompletePasswordResetRequest()
    {     
        bool isRequestCreated = false;

        var model = new ConfirmationModel();
        model.Password = "Password";
        model.ConfirmPassword = "Password";
        model.Token = Token;
        
        isRequestCreated = _securityService.CompletePasswordReset(model);
        
        // Assert
        Assert.True(isRequestCreated);
    } 
}