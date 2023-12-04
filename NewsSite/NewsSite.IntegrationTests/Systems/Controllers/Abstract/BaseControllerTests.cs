using System.Net.Http.Headers;
using System.Net.Http.Json;
using NewsSite.DAL.DTO.Request.Auth;
using NewsSite.DAL.DTO.Response;
using NewsSite.IntegrationTests.Fixtures;

namespace NewsSite.IntegrationTests.Systems.Controllers.Abstract
{
    public abstract class BaseControllerTests
    {
        protected readonly HttpClient _httpClient;
        protected readonly UserRegisterRequest _userRegisterRequest;

        protected BaseControllerTests(WebFactoryFixture fixture)
        {
            _httpClient = fixture.HttpClient;

            _userRegisterRequest = new UserRegisterRequest
            {
                FullName = "testFullName",
                Email = "testEmail@gmail.com",
                Password = "testPassword123$"
            };
        }

        protected async Task AuthenticateAsync()
        {
            _httpClient.DefaultRequestHeaders.Authorization ??= 
                new AuthenticationHeaderValue("Bearer", await GenerateTokenAsync());
        }

        protected async Task<string> GenerateTokenAsync()
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/register", _userRegisterRequest);

            var registrationResponse = await response.Content.ReadFromJsonAsync<NewUserResponse>();

            return registrationResponse!.Token;
        }
    }
}
