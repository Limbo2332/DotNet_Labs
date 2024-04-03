using System.Net.Http.Headers;
using System.Net.Http.Json;
using NewsSite.DAL.DTO.Request.Auth;
using NewsSite.DAL.DTO.Response;
using NewsSite.IntegrationTests.Fixtures;

namespace NewsSite.IntegrationTests.Systems.Controllers.Abstract
{
    public abstract class BaseControllerTests
    {
        protected readonly HttpClient HttpClient;
        protected readonly UserRegisterRequest UserRegisterRequest;

        protected BaseControllerTests(WebFactoryFixture fixture)
        {
            HttpClient = fixture.HttpClient;

            UserRegisterRequest = new UserRegisterRequest
            {
                FullName = "testFullName",
                Email = "testEmail@gmail.com",
                Password = "testPassword123$"
            };
        }

        protected async Task AuthenticateAsync()
        {
            HttpClient.DefaultRequestHeaders.Authorization ??= 
                new AuthenticationHeaderValue("Bearer", await GenerateTokenAsync());
        }

        private async Task<string> GenerateTokenAsync()
        {
            var response = await HttpClient.PostAsJsonAsync("api/auth/register", UserRegisterRequest);

            var registrationResponse = await response.Content.ReadFromJsonAsync<NewUserResponse>();

            return registrationResponse!.Token;
        }
    }
}
