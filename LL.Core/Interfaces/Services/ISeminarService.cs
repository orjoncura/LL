using LL.Core.Models.Arguments;
using LL.Core.Models.ViewModels;

namespace LL.Core.Interfaces.Services
{
    public interface ISeminarService
    {
        Task<List<SeminarViewModel>> CreateSeminar(SeminarRequestModel seminarRequest, int userId);
    }
}
