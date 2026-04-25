using LL.Core.Interfaces.Repositories;

namespace LL.Test.Repositories;

public class ResetPasswordRequestRepositoryTest
{
    private IResetPasswordRequestRepository _resetPasswordRequestRepository { get; set; }

    public ResetPasswordRequestRepositoryTest()
    {
        _resetPasswordRequestRepository = Provider.GetRequiredService<IResetPasswordRequestRepository>();
    }

    [Fact]
    public void Insert_AndGetUserIdByToken_ShouldReturnMatchingUser()
    {
        const int userId = 1;
        var token = _resetPasswordRequestRepository.Insert(userId, "127.0.0.1");
        var resultUserId = _resetPasswordRequestRepository.GetUserIdByToken(token);

        Assert.Equal(userId, resultUserId);
    }
}
