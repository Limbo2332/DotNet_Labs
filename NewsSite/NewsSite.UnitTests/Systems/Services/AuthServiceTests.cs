using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NewsSite.BLL.Exceptions;
using NewsSite.BLL.Interfaces;
using NewsSite.BLL.Services;
using NewsSite.DAL.DTO.Request.Auth;
using NewsSite.DAL.DTO.Response;
using NewsSite.DAL.Repositories.Base;
using NewsSite.UnitTests.Systems.Services.Abstract;
using NewsSite.UnitTests.TestData;
using NewsSite.UnitTests.TestData.PageSettings.Authors;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace NewsSite.UnitTests.Systems.Services
{
    public class AuthServiceTests : BaseServiceTests
    {
        private readonly Mock<IAuthorsRepository> _authorsRepositoryMock;
        private readonly Mock<IConfiguration> _configMock;
        private readonly IAuthService _sut;

        public AuthServiceTests()
        {
            _authorsRepositoryMock = new Mock<IAuthorsRepository>();
            _configMock = new Mock<IConfiguration>();

            _configMock
                .Setup(cm => cm.GetSection(It.IsAny<string>()).Value)
                .Returns(Guid.Empty.ToString());

            _sut = new AuthService(
                _userManagerMock.Object,
                _mapper,
                _configMock.Object,
                _authorsRepositoryMock.Object);
        }

        [Fact]
        public async Task LoginAsync_ShouldThrowException_WhenNoIdentityUser()
        {
            // Arrange
            var exceptionMessage = new NotFoundException(nameof(Author)).Message;
            var userLogin = new UserLoginRequest
            {
                Email = "email"
            };

            // Act
            var action = async () => await _sut.LoginAsync(userLogin);

            // Assert
            await action.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage(exceptionMessage);
        }

        [Fact]
        public async Task LoginAsync_ShouldThrowException_WhenNoUserWithEmail()
        {
            // Arrange
            var exceptionMessage = new NotFoundException(nameof(Author)).Message;
            var userLogin = new UserLoginRequest
            {
                Email = "email"
            };

            var identityUser = new IdentityUser();

            _userManagerMock
                .Setup(um => um.FindByEmailAsync(userLogin.Email))
                .ReturnsAsync(identityUser);

            _userManagerMock
                .Setup(um => um.CheckPasswordAsync(identityUser, userLogin.Password))
                .ReturnsAsync(true);

            // Act
            var action = async () => await _sut.LoginAsync(userLogin);

            // Assert
            await action.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage(exceptionMessage);
        }

        [Fact]
        public async Task LoginAsync_ShouldThrowException_WhenNoCorrectEmailOrPassword()
        {
            // Arrange
            var exceptionMessage = new InvalidEmailOrPasswordException().Message;
            var userLogin = new UserLoginRequest
            {
                Email = "email",
                Password = "password"
            };

            var identityUser = new IdentityUser();

            _userManagerMock
                .Setup(um => um.FindByEmailAsync(userLogin.Email))
                .ReturnsAsync(identityUser);

            _userManagerMock
                .Setup(um => um.CheckPasswordAsync(identityUser, userLogin.Password))
                .ReturnsAsync(false);

            // Act
            var action = async () => await _sut.LoginAsync(userLogin);

            // Assert
            await action.Should()
                .ThrowAsync<InvalidEmailOrPasswordException>()
                .WithMessage(exceptionMessage);
        }

        [Theory]
        [ClassData(typeof(AuthorsTestData))]
        public async Task LoginAsync_ShouldReturnLoginUserResponse_WhenLoginSuccessful(Author author)
        {
            // Arrange
            var userLogin = new UserLoginRequest
            {
                Email = author.Email,
                Password = $"{author.Email} {author.FullName}"
            };

            _userManagerMock
                .Setup(um => um.FindByEmailAsync(userLogin.Email))
                .ReturnsAsync(author.IdentityUser);

            _userManagerMock
                .Setup(um => um.CheckPasswordAsync(author.IdentityUser, userLogin.Password))
                .ReturnsAsync(true);

            _authorsRepositoryMock
                .Setup(ar => ar.GetAuthorByEmailAsync(author.Email))
                .ReturnsAsync(author);

            var loginUserResponse = _mapper.Map<LoginUserResponse>(author);

            // Act
            var result = await _sut.LoginAsync(userLogin);

            // Assert
            using (new AssertionScope())
            {
                result.Email.Should().Be(loginUserResponse.Email);
                result.FullName.Should().Be(loginUserResponse.FullName);
                result.Id.Should().Be(loginUserResponse.Id);
            }
        }

        [Theory]
        [ClassData(typeof(AuthorsTestData))]
        public async Task LoginAsync_ShouldGenerateJWTToken_WhenLoginSuccessful(Author author)
        {
            // Arrange
            var userLogin = new UserLoginRequest
            {
                Email = author.Email,
                Password = $"{author.Email} {author.FullName}"
            };

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                    _configMock.Object.GetSection(It.IsAny<string>()).Value!))
            };

            _userManagerMock
                .Setup(um => um.FindByEmailAsync(userLogin.Email))
                .ReturnsAsync(author.IdentityUser);

            _userManagerMock
                .Setup(um => um.CheckPasswordAsync(author.IdentityUser, userLogin.Password))
                .ReturnsAsync(true);

            _authorsRepositoryMock
                .Setup(ar => ar.GetAuthorByEmailAsync(author.Email))
                .ReturnsAsync(author);

            // Act
            var result = await _sut.LoginAsync(userLogin);

            // Assert
            var claimsPrincipal = new JwtSecurityTokenHandler()
                .ValidateToken(result.Token, tokenValidationParameters, out _);

            var subClaim = claimsPrincipal.Claims.FirstOrDefault(c => c.Type.Contains("name"));
            var emailClaim = claimsPrincipal.Claims.FirstOrDefault(c => c.Type.Contains("email"));

            using (new AssertionScope())
            {
                subClaim!.Value.Should().BeEquivalentTo(result.FullName);
                emailClaim!.Value.Should().BeEquivalentTo(result.Email);
            }
        }

        [Fact]
        public async Task RegisterAsync_ShouldThrowException_WhenNoCorrectEmailOrPassword()
        {
            // Arrange
            var userRegister = new UserRegisterRequest
            {
                Email = "email",
                FullName = "fullName",
            };

            var errors = new List<IdentityError>
            {
                new IdentityError()
                {
                    Description = "Error1",
                    Code = "1"
                },
                new IdentityError()
                {
                    Description = "Error2",
                    Code = "2"
                }
            };

            _userManagerMock
                .Setup(x => x.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed(errors.ToArray()));

            var errorMessage = string.Join(' ', errors.Select(e => e.Description));

            // Act
            var action = async () => await _sut.RegisterAsync(userRegister);

            // Assert
            await action.Should()
                .ThrowAsync<BadRequestException>()
                .WithMessage(errorMessage);
        }

        [Fact]
        public async Task RegisterAsync_ShouldRegisterUser_WhenRegisterSuccessful()
        {
            // Arrange
            var userRegister = new UserRegisterRequest
            {
                Email = "email",
                Password = "password",
                BirthDate = DateTime.UtcNow,
                FullName = "fullName",
            };

            var authors = RepositoriesFakeData.Authors.ToList();

            var authorToAdd = new Author
            {
                BirthDate = userRegister.BirthDate,
                Email = userRegister.Email,
                FullName = userRegister.FullName
            };

            _userManagerMock
                .Setup(x => x.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            _authorsRepositoryMock
                .Setup(ar => ar.AddAsync(It.IsAny<Author>()))
                .Callback(() => authors.Add(authorToAdd));

            var newUserResponse = _mapper.Map<NewUserResponse>(authorToAdd);

            // Act
            var result = await _sut.RegisterAsync(userRegister);

            // Assert
            result.Should().BeEquivalentTo(newUserResponse, opt => opt.Excluding(r => r.Token));
            authors.Should().ContainEquivalentOf(authorToAdd);
        }
    }
}
