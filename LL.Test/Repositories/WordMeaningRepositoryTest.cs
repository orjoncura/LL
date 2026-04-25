using LL.Core.Enums;
using LL.Core.Interfaces.Repositories;

namespace LL.Test.Repositories;

public class WordMeaningRepositoryTest
{
    private IWordRepository _wordRepository { get; set; }
    private IWordMeaningRepository _wordMeaningRepository { get; set; }

    public WordMeaningRepositoryTest()
    {
        _wordRepository = Provider.GetRequiredService<IWordRepository>();
        _wordMeaningRepository = Provider.GetRequiredService<IWordMeaningRepository>();
    }

    [Fact]
    public void Insert_ShouldCreateWordMeaning()
    {
        var wordId = _wordRepository.Insert($"meaning-{Guid.NewGuid():N}", (int)LanguageEnum.English, 1).Id;
        var wordMeaningId = _wordMeaningRepository.Insert(wordId, (int)WordTypeEnum.Noun, 1);

        Assert.True(wordMeaningId > 0);
    }
}
