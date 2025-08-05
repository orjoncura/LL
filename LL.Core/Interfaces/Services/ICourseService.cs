using LL.Core.Models.Arguments;
using LL.Core.Models.ViewModels;

namespace LL.Core.Interfaces.Services
{
    public interface ICourseService
    {
        Task<bool> CreateCourse(CourseRequestModel courseRequest, int userId);
    }
}
