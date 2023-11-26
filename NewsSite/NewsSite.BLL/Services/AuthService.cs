using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NewsSite.BLL.Interfaces;
using NewsSite.BLL.Services.Abstract;
using NewsSite.DAL.DTO.Request.Auth;
using NewsSite.DAL.DTO.Response;
using NewsSite.DAL.Entities;
using NewsSite.DAL.Repositories.Base;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using NewsSite.BLL.Exceptions;

namespace NewsSite.BLL.Services
{
    public class AuthService : BaseService, IAuthService
    {
        private readonly IConfiguration _config;
        private readonly IAuthorsRepository _authorsRepository;

        public AuthService(
            UserManager<IdentityUser> userManager,
            IMapper mapper,
            IConfiguration config,
            IAuthorsRepository authorsRepository)
            : base(userManager, mapper)
        {
            _config = config;
            _authorsRepository = authorsRepository;
        }

        public async Task<LoginUserResponse> LoginAsync(UserLoginRequest userLogin)
        {
            var identityUser = await _userManager.FindByEmailAsync(userLogin.Email)
                ?? throw new NotFoundException(nameof(Author));

            var result = await _userManager.CheckPasswordAsync(identityUser, userLogin.Password);

            if (!result)
            {
                throw new InvalidEmailOrPasswordException();
            }

            var author = await _authorsRepository.GetAuthorByEmailAsync(userLogin.Email);

            var response = _mapper.Map<LoginUserResponse>(author);
            response.Token = GenerateTokenString(author.Email, author.FullName);

            return response;
        }

        public async Task<NewUserResponse> RegisterAsync(UserRegisterRequest userRegister)
        {
            var identityUser = new IdentityUser
            {
                Email = userRegister.Email,
                UserName = userRegister.FullName
            };

            var result = await _userManager.CreateAsync(identityUser, userRegister.Password);

            if (!result.Succeeded)
            {
                var errorsMessages = result.Errors.Select(e => e.Description);

                throw new BadRequestException(string.Join(' ', errorsMessages));
            }

            var author = _mapper.Map<Author>(userRegister);

            await _authorsRepository.AddAsync(author);
            await _authorsRepository.SaveChangesAsync();

            return _mapper.Map<NewUserResponse>(author);
        }

        private string GenerateTokenString(string email, string fullName)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Name, fullName)
            };

            var securityKey = _config.GetSection("JWT:Key").Value!;

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));
            var signingCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config.GetSection("JWT:Issuer").Value,
                audience: _config.GetSection("JWT:Audience").Value,
                expires: DateTime.UtcNow.AddHours(3),
                claims: claims,
                signingCredentials: signingCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
