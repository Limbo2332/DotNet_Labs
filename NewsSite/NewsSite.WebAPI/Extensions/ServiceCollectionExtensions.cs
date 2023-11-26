using Microsoft.AspNetCore.Authentication.JwtBearer;
using NewsSite.BLL.Interfaces;
using NewsSite.BLL.MappingProfiles;
using NewsSite.BLL.Services;
using NewsSite.DAL.Repositories;
using NewsSite.DAL.Repositories.Base;
using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using NewsSite.DAL.Context;
using NewsSite.DAL.DTO.Request.Auth;
using NewsSite.DAL.DTO.Request.Author;
using NewsSite.DAL.DTO.Request.News;
using NewsSite.DAL.DTO.Request.Rubric;
using NewsSite.DAL.DTO.Request.Tag;
using NewsSite.UI.Validators.Request.Author;
using NewsSite.UI.Validators.Request.Auth;
using NewsSite.UI.Validators.Request.News;
using NewsSite.UI.Validators.Request.Rubric;
using NewsSite.UI.Validators.Request.Tag;

namespace NewsSite.UI.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddIdentity(this IServiceCollection services)
        {
            services.AddIdentity<IdentityUser, IdentityRole>(options =>
                {
                    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ ";
                    options.User.RequireUniqueEmail = true;
                })
                .AddEntityFrameworkStores<OnlineNewsContext>()
                .AddDefaultTokenProviders();
        }

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

        public static void RegisterValidators(this IServiceCollection services)
        {
            services.AddScoped<IValidator<UserLoginRequest>, UserLoginRequestValidator>();
            services.AddScoped<IValidator<UserRegisterRequest>, UserRegisterRequestValidator>();

            services.AddScoped<IValidator<UpdatedAuthorRequest>, UpdatedAuthorRequestValidator>();

            services.AddScoped<IValidator<NewNewsRequest>, NewNewsRequestValidator>();
            services.AddScoped<IValidator<UpdateNewsRequest>, UpdateNewsRequestValidator>();

            services.AddScoped<IValidator<NewRubricRequest>, NewRubricRequestValidator>();
            services.AddScoped<IValidator<UpdateRubricRequest>, UpdateRubricRequestValidator>();

            services.AddScoped<IValidator<NewTagRequest>, NewTagRequestValidator>();
            services.AddScoped<IValidator<UpdateTagRequest>, UpdateTagRequestValidator>();
        }
    }
}
