using NewsSite.DAL.DTO.Request;
using NewsSite.DAL.DTO.Response;

namespace NewsSite.BLL.Interfaces
{
    public interface IAuthService
    {
        Task<LoginUserResponse> LoginAsync(UserLoginRequest userLogin);

        Task<NewUserResponse> RegisterAsync(UserRegisterRequest userRegister);
    }
}
