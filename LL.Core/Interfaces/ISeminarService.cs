using LL.SharedDefinitions.Model;

namespace LL.Core.Interfaces
{
    public interface ISeminarService
    {
        Task<List<SeminarViewModel>> CreateSeminar(SeminarRequestModel seminarRequest, int userId);
    }
}
