using LL.Core.Models.Arguments;

namespace LL.Core.Interfaces.Services
{
    public interface ICourseService
    {
        Task<bool> CreateCourse(CourseRequestModel courseRequest, int userId);
    }
}
