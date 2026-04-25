using LL.Core.Interfaces.Repositories;

namespace LL.Test.Repositories;

public class MessageRepositoryTest
{
    private IMessageRepository _messageRepository { get; set; }

    public MessageRepositoryTest()
    {
        _messageRepository = Provider.GetRequiredService<IMessageRepository>();
    }

    [Fact]
    public void Insert_ShouldCreateMessage()
    {
        var messageId = _messageRepository.Insert(
            "user@test.com",
            "Subject",
            "Body",
            1);

        Assert.True(messageId > 0);
    }
}
