using LL.Core.Interfaces.Repositories;

namespace LL.Test.Repositories;

public class NewUserRequestRepositoryTest
{
    private readonly INewUserRequestRepository _newUserRequestRepository;

    public NewUserRequestRepositoryTest()
    {
        _newUserRequestRepository = Provider.GetRequiredService<INewUserRequestRepository>();
    }

    [Fact]
    public void Insert_ShouldCreateToken()
    {
        var token = _newUserRequestRepository.Insert($"new-{Guid.NewGuid():N}@test.com", "127.0.0.1", 1);

        Assert.False(string.IsNullOrWhiteSpace(token));
    }

    [Fact]
    public void GetEmailByToken_ShouldReturnEmail()
    {
        var email = $"token-{Guid.NewGuid():N}@test.com";
        var token = _newUserRequestRepository.Insert(email, "127.0.0.1", 1);

        Assert.Equal(email, _newUserRequestRepository.GetEmailByToken(token));
    }

    [Fact]
    public void HasReachedLimit_ShouldBeFalseForNewEmail()
    {
        var email = $"limit-{Guid.NewGuid():N}@test.com";

        Assert.False(_newUserRequestRepository.HasReachedLimit(email, DateTime.Now.AddDays(-1), 3));
    }

    [Fact]
    public void HasReachedLimit_ShouldBeTrueAfterEnoughAttempts()
    {
        var email = $"limit-hit-{Guid.NewGuid():N}@test.com";
        _newUserRequestRepository.Insert(email, "127.0.0.1", 1);
        _newUserRequestRepository.Insert(email, "127.0.0.1", 1);
        _newUserRequestRepository.Insert(email, "127.0.0.1", 1);
        _newUserRequestRepository.Insert(email, "127.0.0.1", 1);

        Assert.True(_newUserRequestRepository.HasReachedLimit(email, DateTime.Now, 3));
    }
}
