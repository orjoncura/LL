using LL.Data.Contexts;
using LL.Data.Interfaces;
using LL.Data.Model;

namespace LL.Data.Repositories
{
    public class WordRepository(AppDBContext appDBContext) : IWordRepository
    {
        public Word? GetSingleByName(string name) 
        {
            return appDBContext.Words
                .Where(w => w.Name == name.Trim()).FirstOrDefault();
        }

        public int Insert(string name, int fromId, int toId, int userId)
        {
            var word = new Word
            { 
                Name = name.Trim(),
                LanguageFromId = fromId,
                LanguageToId = toId,
                CreatedById = userId,
                CreatedDate = DateTime.Now,
            };

            appDBContext.Add(word);

            return word.Id;
        }

        public bool Exist(string name, int fromId, int toId)
        {
            return appDBContext.Words
                .Where(w => w.Name == name.Trim()
                && w.LanguageFromId == fromId
                && w.LanguageToId == toId).Any();
        }

    }
}
