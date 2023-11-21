using NewsSite.DAL.DTO.Page;
using NewsSite.DAL.DTO.Request.Rubric;
using NewsSite.DAL.DTO.Response;

namespace NewsSite.BLL.Interfaces
{
    public interface IRubricsService
    {
        Task<PageList<RubricResponse>> GetAllRubricsAsync(PageSettings? pageSettings);

        Task<RubricResponse> GetRubricByIdAsync(Guid id);

        Task<RubricResponse> CreateNewRubricAsync(NewRubricRequest newRubric);

        Task<RubricResponse> AddRubricForNewsIdAsync(Guid rubricId, Guid newsId);

        Task<RubricResponse> UpdateRubricAsync(UpdateRubricRequest newRubric);
        
        Task DeleteRubricAsync(Guid id);

        Task DeleteRubricForNewsIdAsync(Guid rubricId, Guid newsId);
    }
}
