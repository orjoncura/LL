using LL.Core.Interfaces.Repositories;

namespace LL.Test.Repositories;

public class NewUserRequestRepositoryTest
{
    private INewUserRequestRepository _newUserRequestRepository { get; set; }

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
}
