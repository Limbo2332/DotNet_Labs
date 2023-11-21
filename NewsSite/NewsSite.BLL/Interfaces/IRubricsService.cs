using NewsSite.DAL.DTO.Page;
using NewsSite.DAL.DTO.Request;
using NewsSite.DAL.DTO.Response;

namespace NewsSite.BLL.Interfaces
{
    public interface IRubricsService
    {
        Task<PageList<RubricResponse>> GetAllRubricsAsync(PageSettings? pageSettings);

        Task<RubricResponse> GetRubricByIdAsync(Guid id);

        Task<RubricResponse> CreateNewRubricAsync(NewRubricRequest newRubric);

        Task<RubricResponse> UpdateRubricAsync(UpdateRubricRequest newRubric);
        
        Task DeleteRubricAsync(Guid id);
    }
}
