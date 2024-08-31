using LL.Data.Model;

namespace LL.Data.Interfaces
{
    public interface IWordRepository
    {
        public Word? GetSingleByName(string name);
        int Insert(string name, int fromId, int toId, int userId);
        bool Exist(string name, int fromId, int toId);
    }
}
