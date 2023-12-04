using NewsSite.DAL.Repositories;

namespace NewsSite.IntegrationTests.Fixtures
{
    [CollectionDefinition(nameof(DbContextAndUserManagerFixture))]
    public class DbContextAndUserManagementFixtureCollection 
        : ICollectionFixture<DbContextAndUserManagerFixture>
    {
    }
}
