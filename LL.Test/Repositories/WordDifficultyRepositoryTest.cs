using LL.Core.Enums;
using LL.Core.Interfaces.Extensions;
using LL.Core.Interfaces.Repositories;

namespace LL.Test.Repositories;

public class WordDifficultyRepositoryTest
{
    private IWordRepository _wordRepository { get; set; }
    private IWordDifficultyRepository _wordDifficultyRepository { get; set; }
    private IEncryptionService _encryptionService { get; set; }

    public WordDifficultyRepositoryTest()
    {
        _wordRepository = Provider.GetRequiredService<IWordRepository>();
        _wordDifficultyRepository = Provider.GetRequiredService<IWordDifficultyRepository>();
        _encryptionService = Provider.GetRequiredService<IEncryptionService>();
    }

    [Fact]
    public void SetWordDifficulty_ShouldReturnTrue()
    {
        var wordId = _wordRepository.Insert($"difficulty-{Guid.NewGuid():N}", (int)LanguageEnum.English, 1).Id;
        var encryptedWordId = _encryptionService.Encrypt(wordId);

        var result = _wordDifficultyRepository.SetWordDifficulty((int)ImportanceRatingEnum.High, encryptedWordId, 1);

        Assert.True(result);
    }
}
