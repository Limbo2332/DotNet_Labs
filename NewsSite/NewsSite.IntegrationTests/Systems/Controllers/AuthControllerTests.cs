using System.Net;
using System.Net.Http.Json;
using NewsSite.DAL.Constants;
using NewsSite.DAL.Context.Constants;
using NewsSite.DAL.DTO;
using NewsSite.DAL.DTO.Request.Auth;
using NewsSite.DAL.DTO.Response;
using NewsSite.IntegrationTests.Fixtures;
using NewsSite.IntegrationTests.Systems.Controllers.Abstract;
using Newtonsoft.Json;

namespace NewsSite.IntegrationTests.Systems.Controllers
{
    [Collection(nameof(WebFactoryFixture))]
    public class AuthControllerTests(WebFactoryFixture fixture) : BaseControllerTests(fixture)
    {
        [Fact]
        public async Task Login_ShouldReturnBadRequest_WhenValidationIsWrong()
        {
            // Arrange
            var userLoginRequest = new UserLoginRequest
            {
                Email = string.Empty,
                Password = string.Empty,
            };

            // Act
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", userLoginRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var responseContent = await response.Content.ReadAsStringAsync();
            var badRequestModel = JsonConvert.DeserializeObject<BadRequestModel>(responseContent);

            badRequestModel.Should().NotBeNull();
            badRequestModel!.Errors.Should().NotBeEmpty();
            badRequestModel.Message.Should().Be(ValidationMessages.VALIDATION_MESSAGE_RESPONSE);
            badRequestModel.HttpStatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Login_ShouldReturnNotFound_WhenNoSuchUser()
        {
            // Arrange
            var userLoginRequest = new UserLoginRequest
            {
                Email = "userNotExists@gmail.com",
                Password = "userNotExists1!"
            };

            // Act
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", userLoginRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var responseContent = await response.Content.ReadAsStringAsync();
            var badRequestModel = JsonConvert.DeserializeObject<BadRequestModel>(responseContent);

            badRequestModel.Should().NotBeNull();
            badRequestModel!.Errors.Should().NotBeEmpty();
            badRequestModel.HttpStatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Login_ShouldReturnInvalidEmailOrPasswordRequest_WhenValidationIsWrong()
        {
            // Arrange
            await AuthenticateAsync();

            var userLoginRequest = new UserLoginRequest
            {
                Email = _userRegisterRequest.Email,
                Password = _userRegisterRequest.Password + _userRegisterRequest.Email
            };

            // Act
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", userLoginRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var responseContent = await response.Content.ReadAsStringAsync();
            var badRequestModel = JsonConvert.DeserializeObject<BadRequestModel>(responseContent);

            badRequestModel.Should().NotBeNull();
            badRequestModel!.Errors.Should().NotBeEmpty();
            badRequestModel.Errors.Should().Contain(ValidationMessages.INVALID_EMAIL_OR_PASSWORD_MESSAGE);
            badRequestModel.HttpStatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Login_ShouldReturnOkResult_WhenUserExists()
        {
            // Arrange
            await AuthenticateAsync();

            var userLoginRequest = new UserLoginRequest
            {
                Email = _userRegisterRequest.Email,
                Password = _userRegisterRequest.Password
            };

            // Act
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", userLoginRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseContent = await response.Content.ReadFromJsonAsync<LoginUserResponse>();

            responseContent.Should().NotBeNull();
            responseContent!.Email.Should().BeEquivalentTo(userLoginRequest.Email);
        }
        
        [Fact]
        public async Task Register_ShouldReturnBadRequest_WhenValidationIsWrong()
        {
            // Arrange
            var userRegisterRequest = new UserRegisterRequest
            {
                Email = string.Empty,
                Password = string.Empty,
            };

            // Act
            var response = await _httpClient.PostAsJsonAsync("api/auth/register", userRegisterRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var responseContent = await response.Content.ReadAsStringAsync();
            var badRequestModel = JsonConvert.DeserializeObject<BadRequestModel>(responseContent);

            badRequestModel.Should().NotBeNull();
            badRequestModel!.Errors.Should().NotBeEmpty();
            badRequestModel.Message.Should().Be(ValidationMessages.VALIDATION_MESSAGE_RESPONSE);
            badRequestModel.HttpStatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        
        [Fact]
        public async Task Register_ShouldReturnBadRequest_WhenTheUser()
        {
            // Arrange
            var userRegisterRequest = new UserRegisterRequest
            {
                Email = string.Empty,
                Password = string.Empty,
            };

            // Act
            var response = await _httpClient.PostAsJsonAsync("api/auth/register", userRegisterRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var responseContent = await response.Content.ReadAsStringAsync();
            var badRequestModel = JsonConvert.DeserializeObject<BadRequestModel>(responseContent);

            badRequestModel.Should().NotBeNull();
            badRequestModel!.Errors.Should().NotBeEmpty();
            badRequestModel.Message.Should().Be(ValidationMessages.VALIDATION_MESSAGE_RESPONSE);
            badRequestModel.HttpStatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Register_ShouldReturnCreatedResult()
        {
            // Arrange
            var userRegisterRequest = new UserRegisterRequest
            {
                Email = "registerEmail@gmail.com",
                Password = "registerPassword1!",
                FullName = "registerFullName",
                BirthDate = DateTime.UtcNow.AddYears(-ConfigurationConstants.MIN_YEARS_TO_REGISTER)
            };

            // Act
            var response = await _httpClient.PostAsJsonAsync(
                "api/auth/register", 
                userRegisterRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var responseContent = await response.Content.ReadFromJsonAsync<NewUserResponse>();

            responseContent.Should().NotBeNull();
            responseContent!.Email.Should().BeEquivalentTo(userRegisterRequest.Email);
            responseContent.FullName.Should().BeEquivalentTo(userRegisterRequest.FullName);
            responseContent.BirthDate.Should().Be(userRegisterRequest.BirthDate);
        }
    }
}
