using LL.SharedDefinitions.Model;

namespace LL.Core.Interfaces
{
    public interface IChatService
    {
        Task<SeminarViewModel> CreateSeminar(SeminarRequestModel seminarRequest);
    }
}
