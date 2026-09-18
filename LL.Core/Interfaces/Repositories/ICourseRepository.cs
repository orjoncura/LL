using LL.Core.Models.ViewModels;

namespace LL.Core.Interfaces.Repositories;

public interface ICourseRepository
{
    int Insert(string title, string value, int fromId, int toId, int userId);
    bool UpdateValue(int courseId, string value, int userId);
    bool MarkCourseAsCompleted(int courseId);
    bool DeleteCourseWord(string moduleId, string wordId, int userId);
    bool DeleteCourseById(string id, int userId);
    List<CourseViewModel> GetCoursesByUserId(int userId);
    CourseViewModel? GetById(string encryptedId);
}