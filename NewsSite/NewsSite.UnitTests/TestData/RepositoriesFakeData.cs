using Microsoft.AspNetCore.Identity;
using NewsSite.DAL.Context.Constants;

namespace NewsSite.UnitTests.TestData
{
    public class RepositoriesFakeData
    {
        public const int ITEMS_COUNT = 5;

        public static List<Author> Authors = new Faker<Author>()
            .UseSeed(1)
            .RuleFor(a => a.Id, f => f.Random.Guid())
            .RuleFor(a => a.CreatedAt, _ => DateTime.UtcNow)
            .RuleFor(a => a.UpdatedAt, _ => DateTime.UtcNow)
            .RuleFor(a => a.Email, f => f.Internet.Email())
            .RuleFor(a => a.FullName, f => f.Internet.UserName())
            .RuleFor(a => a.Sex, f => f.Random.Bool())
            .RuleFor(a => a.PublicInformation,
                f => f.Lorem.Paragraph())
            .RuleFor(a => a.BirthDate, 
                f => f.Date.Between(
                    DateTime.UtcNow.AddYears(-ConfigurationConstants.MIN_YEARS_TO_REGISTER * 2), 
                    DateTime.UtcNow).AddYears(-ConfigurationConstants.MIN_YEARS_TO_REGISTER))
            .RuleFor(a => a.IdentityUser, (_, a) => new IdentityUser(a.FullName))
            .Generate(ITEMS_COUNT);

        public static List<News> News = new Faker<News>()
            .UseSeed(1)
            .RuleFor(n => n.Id, f => f.Random.Guid())
            .RuleFor(n => n.CreatedAt, _ => DateTime.UtcNow)
            .RuleFor(n => n.UpdatedAt, _ => DateTime.UtcNow)
            .RuleFor(n => n.CreatedBy, f => f.PickRandom(Authors).Id)
            .RuleFor(n => n.Subject, f => f.Lorem.Sentence())
            .RuleFor(n => n.Content, f => f.Lorem.Paragraph())
            .Generate(ITEMS_COUNT);

        public static List<Rubric> Rubrics = new Faker<Rubric>()
            .UseSeed(1)
            .RuleFor(r => r.Id, f => f.Random.Guid())
            .RuleFor(r => r.CreatedAt, _ => DateTime.UtcNow)
            .RuleFor(r => r.UpdatedAt, _ => DateTime.UtcNow)
            .RuleFor(r => r.Name, f => f.Lorem.Word())
            .Generate(ITEMS_COUNT);

        public static List<Tag> Tags = new Faker<Tag>()
            .UseSeed(1)
            .RuleFor(t => t.Id, f => f.Random.Guid())
            .RuleFor(t => t.CreatedAt, _ => DateTime.UtcNow)
            .RuleFor(t => t.UpdatedAt, _ => DateTime.UtcNow)
            .RuleFor(t => t.Name, f => f.Lorem.Word())
            .Generate(ITEMS_COUNT);

        public static List<NewsRubrics> NewsRubrics = new Faker<NewsRubrics>()
            .UseSeed(1)
            .RuleFor(nr => nr.NewsId, f => f.PickRandom(News).Id)
            .RuleFor(nr => nr.RubricId, f => f.PickRandom(Rubrics).Id)
            .Generate(ITEMS_COUNT * 2)
            .DistinctBy(nr => new { nr.RubricId, nr.NewsId })
            .ToList();

        public static List<NewsTags> NewsTags = new Faker<NewsTags>()
            .UseSeed(1)
            .RuleFor(nt => nt.NewsId, f => f.PickRandom(News).Id)
            .RuleFor(nt => nt.TagId, f => f.PickRandom(Tags).Id)
            .Generate(ITEMS_COUNT * 2)
            .DistinctBy(nt => new { nt.TagId, nt.NewsId })
            .ToList();
    }
}
