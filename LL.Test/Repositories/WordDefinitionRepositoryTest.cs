using LL.Core.Enums;
using LL.Core.Interfaces.Repositories;

namespace LL.Test.Repositories;

public class WordDefinitionRepositoryTest
{
    private IWordRepository _wordRepository { get; set; }
    private IWordMeaningRepository _wordMeaningRepository { get; set; }
    private IWordDefinitionRepository _wordDefinitionRepository { get; set; }

    public WordDefinitionRepositoryTest()
    {
        _wordRepository = Provider.GetRequiredService<IWordRepository>();
        _wordMeaningRepository = Provider.GetRequiredService<IWordMeaningRepository>();
        _wordDefinitionRepository = Provider.GetRequiredService<IWordDefinitionRepository>();
    }

    [Fact]
    public void Insert_ShouldCreateWordDefinition()
    {
        var wordId = _wordRepository.Insert($"definition-{Guid.NewGuid():N}", (int)LanguageEnum.English, 1).Id;
        var wordMeaningId = _wordMeaningRepository.Insert(wordId, (int)WordTypeEnum.Noun, 1);
        var definitionId = _wordDefinitionRepository.Insert("A sample definition", wordMeaningId, 1);

        Assert.True(definitionId > 0);
    }
}
