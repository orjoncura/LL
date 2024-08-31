using LL.SharedDefinitions.Model;

namespace LL.Core.Interfaces
{
    public interface IChatService
    {
        Task<List<SeminarViewModel>> CreateSeminar(SeminarRequestModel seminarRequest);
    }
}
