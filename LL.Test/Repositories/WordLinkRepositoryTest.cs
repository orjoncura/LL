using LL.Core.Enums;
using LL.Core.Interfaces.Repositories;
using LL.Resources.Contexts;
using LL.Resources.Models;

namespace LL.Test.Repositories;

public class WordLinkRepositoryTest
{
    private IWordRepository _wordRepository { get; set; }
    private IWordLinkRepository _wordLinkRepository { get; set; }
    private AppDbContext _dbContext { get; set; }

    public WordLinkRepositoryTest()
    {
        _wordRepository = Provider.GetRequiredService<IWordRepository>();
        _wordLinkRepository = Provider.GetRequiredService<IWordLinkRepository>();
        _dbContext = Provider.GetRequiredService<AppDbContext>();
    }

    [Fact]
    public void Insert_ShouldReturnExistingWordLink()
    {
        var wordId = _wordRepository.Insert($"link-{Guid.NewGuid():N}", (int)LanguageEnum.English, 1).Id;

        var existing = new WordLink
        {
            WordId = wordId,
            Value = "hola",
            LanguageId = (int)LanguageEnum.Spanish,
            IsActive = true,
            CreatedById = 1,
            CreatedDate = DateTime.Now
        };
        _dbContext.WordLinks.Add(existing);
        _dbContext.SaveChanges();

        var result = _wordLinkRepository.Insert(
            wordId,
            "hola",
            (int)LanguageEnum.English,
            (int)LanguageEnum.Spanish,
            1);

        Assert.Equal(existing.Id, result.Id);
    }
}
