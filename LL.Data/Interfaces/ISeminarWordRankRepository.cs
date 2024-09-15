using LL.Data.Model;

namespace LL.Data.Interfaces;

public interface ISeminarWordRankRepository
{
    int Insert(string name, int userId);
}