using LL.Data.Contexts;
using LL.Data.Interfaces;
using LL.Data.Model;

namespace LL.Data.Repositories
{
    public class WordRepository(AppDBContext appDBContext) : IWordRepository
    {
        public Word? GetSingleByName(string name) =>
            appDBContext.Words.FirstOrDefault(w => 
                w.Name == name.Trim());
        
        public int Insert(string name, string translation, string definition, int typeId, int fromId, int toId, int userId)
        {
            var word = new Word
            { 
                Name = name.Trim(),
                Definition = definition.Trim(),
                TypeId = typeId,
                IsActive = true,
                CreatedById = userId,
                CreatedDate = DateTime.Now,
            };

            appDBContext.Add(word);
            appDBContext.SaveChanges();

            return word.Id;
        }

        public bool Exist(string name, int fromId, int toId) => 
            appDBContext.Words.Any(w => 
                w.Name.ToLower() == name.Trim().ToLower());
    }
}
