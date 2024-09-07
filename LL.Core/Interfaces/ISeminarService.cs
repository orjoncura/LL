using LL.SharedDefinitions.Models;

namespace LL.Core.Interfaces
{
    public interface ISeminarService
    {
        Task<List<SeminarViewModel>> CreateSeminar(SeminarRequestModel seminarRequest, int userId);
    }
}
