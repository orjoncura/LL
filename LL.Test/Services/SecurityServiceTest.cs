using LL.Core.Constants;
using LL.Core.Interfaces.Repositories;
using LL.Core.Interfaces.Services;
using LL.Core.Models.Arguments;
using LL.Resources.Contexts;
using LL.Resources.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;

namespace LL.Test.Services;

public class SecurityServiceTest
{
    private readonly ISecurityService _securityService;
    private readonly IUserRepository _userRepository;
    private readonly INewUserRequestRepository _newUserRequestRepository;
    private readonly IResetPasswordRequestRepository _resetPasswordRequestRepository;

    public SecurityServiceTest()
    {     
        var mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.Setup(c => c["Security:JwtKey"]).Returns("test-secret-key");
        mockConfiguration.Setup(c => c["Security:Issuer"]).Returns("test-issuer");
        mockConfiguration.Setup(c => c["Security:Audience"]).Returns("test-audience");
        mockConfiguration.Setup(c => c[Secrets.EncryptionKey])
            .Returns("edTWS52cRCrRB4NDDCwT6mY6dMcWwa3n");

        var services = Provider.GetRequiredService();
        services.RemoveAll<IConfiguration>();
        services.AddSingleton(mockConfiguration.Object);
        
        var serviceProvider = services.BuildServiceProvider();
        _securityService = serviceProvider.GetRequiredService<ISecurityService>();
        _userRepository = serviceProvider.GetRequiredService<IUserRepository>();
        _newUserRequestRepository = serviceProvider.GetRequiredService<INewUserRequestRepository>();
        _resetPasswordRequestRepository = serviceProvider.GetRequiredService<IResetPasswordRequestRepository>();
        
        var context = serviceProvider.GetRequiredService<AppDbContext>();
        context.Database.EnsureCreated();

        if (!context.Users.Any(u => u.Email == "admin@admin.com"))
        {
            context.Users.Add(new User()
            {
                Email = "admin@admin.com",
                PasswordHash = "password",
                Salt = "salt",
                IsActive = true,
                CreatedDate = DateTime.Now
            });
            context.SaveChanges();
        }
    }
    
    [Fact]
    public void RegisterUser_ShouldRegisterNewUserRequest()
    {     
        var email = $"test-{Guid.NewGuid():N}@test.com";
        var model = new NewUserModel
        {
            Email = email,
            ConfirmEmail = email
        };
        
        Assert.True(_securityService.RegisterUser(model, "123.123.123.123", "http://localhost:3000"));
    } 
    
    [Fact]
    public void CompleteUserRegistration_ShouldRegisterNewUser()
    {
        var email = $"complete-{Guid.NewGuid():N}@test.com";
        var token = _newUserRequestRepository.Insert(email, "127.0.0.1", 1);

        var model = new ConfirmationModel
        {
            Email = email,
            Password = "Password123!",
            ConfirmPassword = "Password123!",
            Token = token
        };
        
        Assert.True(_securityService.CompleteUserRegistration(model));
        Assert.NotNull(_userRepository.GetByEmail(email));
    } 
    
    [Fact]
    public void ResetPassword_ShouldCreateNewResetPasswordRequest()
    {
        var email = $"reset-{Guid.NewGuid():N}@test.com";
        _userRepository.Insert(email, "Password123!", 1);
        
        Assert.True(_securityService.ResetPassword(email, "123.123.123.123", "http://localhost:3000"));
    } 
    
    [Fact]
    public void CompletePasswordReset_ShouldCompletePasswordResetRequest()
    {
        var email = $"reset-complete-{Guid.NewGuid():N}@test.com";
        var userId = _userRepository.Insert(email, "Password123!", 1);
        var token = _resetPasswordRequestRepository.Insert(userId, "127.0.0.1");

        var model = new ConfirmationModel
        {
            Email = email,
            Password = "NewPassword123!",
            ConfirmPassword = "NewPassword123!",
            Token = token
        };
        
        Assert.True(_securityService.CompletePasswordReset(model));
    }

    [Fact]
    public void GetProfileDetails_ShouldReturnUserProfile()
    {
        var email = $"profile-{Guid.NewGuid():N}@test.com";
        var userId = _userRepository.Insert(email, "Password123!", 1);

        var profile = _securityService.GetProfileDetails(userId);

        Assert.Equal(email, profile.Email);
        Assert.False(string.IsNullOrWhiteSpace(profile.DateCreated?.AsString));
    }

    [Fact]
    public void GetProfileDetails_WithUnknownUser_ShouldReturnEmptyProfile()
    {
        var profile = _securityService.GetProfileDetails(-999);

        Assert.True(string.IsNullOrWhiteSpace(profile.Email));
    }
}
