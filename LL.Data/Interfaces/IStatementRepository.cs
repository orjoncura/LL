using LL.Data.Model;

namespace LL.Data.Interfaces
{
    public interface IStatementRepository
    {
        List<Statement> GetByWordId(int wordId);
        int Insert(int wordId, string original, string translated, int fromId, int toId, int userId);
        bool InsertRange(List<Statement> statements);
    }
}
