using LL.Core.Interfaces.Repositories;

namespace LL.Test.Repositories;

public class UserRepositoryTest
{
    private IUserRepository _userRepository { get; set; }

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
}
