namespace LL.Core.Interfaces.Repositories;

public interface ICourseRepository
{
    int Insert(string value, int fromId, int toId, int userId);
}