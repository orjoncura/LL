using LL.Core.Interfaces.Repositories;

namespace LL.Test.Repositories;

public class ResetPasswordRequestRepositoryTest
{
    private readonly IResetPasswordRequestRepository _resetPasswordRequestRepository;

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

    [Fact]
    public void HasReachedLimit_ShouldBeFalseInitially()
    {
        var userId = Random.Shared.Next(10000, 20000);

        Assert.False(_resetPasswordRequestRepository.HasReachedLimit(userId, DateTime.Now.AddDays(-1), 3));
    }

    [Fact]
    public void HasReachedLimit_ShouldBeTrueAfterEnoughAttempts()
    {
        var userId = Random.Shared.Next(20000, 30000);
        _resetPasswordRequestRepository.Insert(userId, "127.0.0.1");
        _resetPasswordRequestRepository.Insert(userId, "127.0.0.1");
        _resetPasswordRequestRepository.Insert(userId, "127.0.0.1");
        _resetPasswordRequestRepository.Insert(userId, "127.0.0.1");

        Assert.True(_resetPasswordRequestRepository.HasReachedLimit(userId, DateTime.Now, 3));
    }
}
