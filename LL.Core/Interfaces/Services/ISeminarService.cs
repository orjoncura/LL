using LL.Core.Models.Arguments;
using LL.Core.Models.ViewModels;

namespace LL.Core.Interfaces.Services
{
    public interface ISeminarService
    {
        Task<SeminarViewModel> CreateSeminar(SeminarRequestModel seminarRequest, int userId);
    }
}
