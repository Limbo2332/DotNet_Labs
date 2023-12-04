using NewsSite.DAL.Repositories;

namespace NewsSite.IntegrationTests.Fixtures
{
    [CollectionDefinition(nameof(WebFactoryFixture))]
    public class WebFactoryFixtureCollection 
        : ICollectionFixture<WebFactoryFixture>
    {
    }
}
