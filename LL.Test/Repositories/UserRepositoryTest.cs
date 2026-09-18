using LL.Core.Interfaces.Repositories;
using LL.Core.Model.DataTransferObjects;
using LL.Core.Models.Arguments;
using Microsoft.Extensions.DependencyInjection;

namespace LL.Test.Repositories;

public class UserRepositoryTest
{
    private readonly IUserRepository _userRepository;

    public UserRepositoryTest()
    {
        _userRepository = Provider.GetRequiredService<IUserRepository>();
    }

    [Fact]
    public void Insert_ShouldCreateUser()
    {
        var email = $"user-{Guid.NewGuid():N}@test.com";
        var userId = _userRepository.Insert(email, "Password123!", 1);

        Assert.True(userId > 0);
        Assert.Equal(email, _userRepository.GetByEmail(email)?.Email);
    }

    [Fact]
    public void GetById_ShouldReturnInsertedUser()
    {
        var email = $"byid-{Guid.NewGuid():N}@test.com";
        var userId = _userRepository.Insert(email, "Password123!", 1);

        var user = _userRepository.GetById(userId);

        Assert.NotNull(user);
        Assert.Equal(email, user.Email);
    }

    [Fact]
    public void UpdatePassword_ShouldAllowLoginWithNewPassword()
    {
        var email = $"pwd-{Guid.NewGuid():N}@test.com";
        var userId = _userRepository.Insert(email, "OldPassword1!", 1);

        Assert.True(_userRepository.UpdatePassword(userId, "NewPassword1!", 1));

        var token = _userRepository.GetAuthenticationToken(
            new LoginModel { Email = email, Password = "NewPassword1!" },
            new TokenConfigModel(
                "aP9vB3kL6mN8qR2tU5xY7zA1cD4fG6hJ9kL2mN5pQ8rS1vW4yZ7xC3vB6nM9qT2",
                "test-issuer",
                "test-audience",
                "60"));

        Assert.False(string.IsNullOrWhiteSpace(token.Token));
    }

    [Fact]
    public void GetAuthenticationToken_WithWrongPassword_ShouldReturnEmptyToken()
    {
        var email = $"wrong-{Guid.NewGuid():N}@test.com";
        _userRepository.Insert(email, "Password123!", 1);

        var token = _userRepository.GetAuthenticationToken(
            new LoginModel { Email = email, Password = "Nope" },
            new TokenConfigModel(
                "aP9vB3kL6mN8qR2tU5xY7zA1cD4fG6hJ9kL2mN5pQ8rS1vW4yZ7xC3vB6nM9qT2",
                "test-issuer",
                "test-audience",
                "60"));

        Assert.True(string.IsNullOrWhiteSpace(token.Token));
    }
}
