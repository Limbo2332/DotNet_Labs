using Microsoft.AspNetCore.Identity;
using NewsSite.DAL.Context.Constants;

namespace NewsSite.UnitTests.TestData
{
    public class RepositoriesFakeData
    {
        public const int AUTHORS_ITEMS_COUNT = 5;

        public static List<Author> Authors = new Faker<Author>()
            .UseSeed(1)
            .RuleFor(a => a.Id, f => f.Random.Guid())
            .RuleFor(a => a.CreatedAt, f => DateTime.UtcNow)
            .RuleFor(a => a.UpdatedAt, f => DateTime.UtcNow)
            .RuleFor(a => a.Email, f => f.Internet.Email())
            .RuleFor(a => a.FullName, f => f.Internet.UserName())
            .RuleFor(a => a.Sex, f => f.Random.Bool())
            .RuleFor(a => a.PublicInformation,
                f => f.Lorem.Paragraph())
            .RuleFor(a => a.BirthDate, 
                f => f.Date.Between(
                    DateTime.UtcNow.AddYears(-ConfigurationConstants.MIN_YEARS_TO_REGISTER * 2), 
                    DateTime.UtcNow).AddYears(-ConfigurationConstants.MIN_YEARS_TO_REGISTER))
            .RuleFor(a => a.IdentityUser, (f, a) => new IdentityUser(a.FullName))
            .Generate(AUTHORS_ITEMS_COUNT);
    }
}
