using GenericCollection.BLL.Interfaces;
using GenericCollection.Presentation.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace GenericCollection.Presentation
{
    public class Program
    {
        static void Main(string[] args)
        {
            IServiceCollection services = new ServiceCollection();

            services.AddServices();

            IServiceProvider serviceProvider = services.BuildServiceProvider();

            serviceProvider.GetService<IRunner>()?.Run();
        }
    }
}