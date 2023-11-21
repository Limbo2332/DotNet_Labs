using Microsoft.AspNetCore.Authentication.JwtBearer;
using NewsSite.BLL.Interfaces;
using NewsSite.BLL.MappingProfiles;
using NewsSite.BLL.Services;
using NewsSite.DAL.Repositories;
using NewsSite.DAL.Repositories.Base;
using System.Reflection;

namespace NewsSite.UI.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddAuthenticationWithJwt(this IServiceCollection services, IConfiguration config)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    RequireExpirationTime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = config.GetSection("JWT:Issuer").Value,
                    ValidAudience = config.GetSection("JWT:Audience").Value
                };
            });
        }
        public static void RegisterRepositories(this IServiceCollection services)
        {
            services.AddScoped<IAuthorsRepository, AuthorsRepository>();
            services.AddScoped<INewsRepository, NewsRepository>();
            services.AddScoped<IRubricsRepository, RubricsRepository>();
            services.AddScoped<ITagsRepository, TagsRepository>();
        }

        public static void RegisterAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<AuthorsProfile>();
                cfg.AddProfile<NewsProfile>();
                cfg.AddProfile<RubricsProfile>();
                cfg.AddProfile<TagsProfile>();
            }, Assembly.GetAssembly(typeof(AuthorsProfile)));
        }

        public static void AddCustomServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IAuthorsService, AuthorsService>();
            services.AddScoped<INewsService, NewsService>();
            services.AddScoped<IRubricsService, RubricsService>();
            services.AddScoped<ITagsService, TagsService>();
        }
    }
}
