using Microsoft.AspNetCore.Mvc;
using NewsSite.BLL.Interfaces;
using NewsSite.BLL.Services;
using NewsSite.UI.Controllers;
using System.Net;
using NewsSite.DAL.DTO.Request.Auth;
using NewsSite.DAL.DTO.Response;

namespace NewsSite.UnitTests.Systems.Controllers
{
    public class AuthControllerTests
    {
        private readonly IAuthService _authService;
        private readonly AuthController _sut;

        public AuthControllerTests()
        {
            _authService = Substitute.For<IAuthService>();

            _sut = new AuthController(_authService);
        }

        [Fact]
        public async Task LoginAsync_ShouldBeSuccessful()
        {
            // Arrange
            var userLoginRequest = Substitute.For<UserLoginRequest>();
            var loginUserResponse = Substitute.For<LoginUserResponse>();

            _authService
                .LoginAsync(userLoginRequest)
                .Returns(loginUserResponse);

            // Act
            var result = await _sut.Login(userLoginRequest);
            var response = result.Result as OkObjectResult;

            // Assert
            using (new AssertionScope())
            {
                _authService.ReceivedCalls().Count().Should().Be(1);

                response.Should().NotBeNull();
                response!.Value.Should().Be(loginUserResponse);
                response.StatusCode.Should().Be((int)HttpStatusCode.OK);
            }
        }

        [Fact]
        public async Task RegisterAsync_ShouldBeSuccessful()
        {
            // Arrange
            var userRegisterRequest = Substitute.For<UserRegisterRequest>();
            var newsUserResponse = Substitute.For<NewUserResponse>();

            _authService
                .RegisterAsync(userRegisterRequest)
                .Returns(newsUserResponse);

            // Act
            var result = await _sut.Register(userRegisterRequest);
            var response = result.Result as CreatedResult;

            // Assert
            using (new AssertionScope())
            {
                _authService.ReceivedCalls().Count().Should().Be(1);

                response.Should().NotBeNull();
                response!.Value.Should().Be(newsUserResponse);
                response.StatusCode.Should().Be((int)HttpStatusCode.Created);
            }
        }
    }
}
