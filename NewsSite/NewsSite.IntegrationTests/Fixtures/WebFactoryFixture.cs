using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NewsSite.UnitTests.TestData;

namespace NewsSite.IntegrationTests.Fixtures
{
    public class WebFactoryFixture : IDisposable
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly IServiceScope _scope;

        public OnlineNewsContext DbContext { get; }

        public UserManager<IdentityUser> UserManager { get; }

        public HttpClient HttpClient { get; }

        public WebFactoryFixture()
        {
            _factory = InitializeFactory();

            _scope = _factory.Services.CreateScope();

            DbContext = InitializeContext();
            UserManager = InitializeUserManager();
            HttpClient = _factory.CreateClient();
        }

        private WebApplicationFactory<Program> InitializeFactory()
        {
            return new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureTestServices(services =>
                    {
                        services.RemoveAll(typeof(DbContextOptions<OnlineNewsContext>));
                        services.AddDbContext<OnlineNewsContext>(options =>
                        {
                            options.UseInMemoryDatabase("InMemoryDatabase");
                        });
                    });
                });
        }

        private OnlineNewsContext InitializeContext()
        {
            var context = _scope.ServiceProvider.GetRequiredService<OnlineNewsContext>();

            if (context.Database.EnsureCreated() && !context.Authors.Any())
            {
                var authors = RepositoriesFakeData.Authors.ToList();

                context.Authors.AddRange(authors);
                context.News.AddRange(RepositoriesFakeData.News.ToList());
                context.Rubrics.AddRange(RepositoriesFakeData.Rubrics.ToList());
                context.Tags.AddRange(RepositoriesFakeData.Tags.ToList());
                context.NewsTags.AddRange(RepositoriesFakeData.NewsTags.ToList());
                context.NewsRubrics.AddRange(RepositoriesFakeData.NewsRubrics.ToList());

                context.SaveChanges();
            }

            return context;
        }

        private UserManager<IdentityUser> InitializeUserManager()
        {
            var userManager = _scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

            if (userManager.Users.Any(u => u.NormalizedEmail == null))
            {
                foreach (var author in RepositoriesFakeData.Authors)
                {
                    author.IdentityUser.NormalizedEmail = userManager.NormalizeEmail(author.Email);
                    author.IdentityUser.NormalizedUserName = userManager.NormalizeName(author.FullName);
                    userManager.UpdateAsync(author.IdentityUser);
                }
            }

            return userManager;
        }

        public void Dispose()
        {
            _factory.Dispose();
        }
    }
}
