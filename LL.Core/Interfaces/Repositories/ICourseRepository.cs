using LL.Core.Models.ViewModels;

namespace LL.Core.Interfaces.Repositories;

public interface ICourseRepository
{
    int Insert(string value, int fromId, int toId, int userId);
    bool DeleteCourseWord(int courseId, int wordId, int userId);
    bool DeleteCourseById(string id, int userId);
    List<CourseViewModel> GetCoursesByUserId(int userId);
}