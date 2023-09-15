using GenericCollection.BLL.Interfaces;
using GenericCollection.BLL.Services;
using GenericCollection.DAL.Repositories;
using GenericCollection.DAL.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace GenericCollection.Presentation.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IWriter, Writer>();
            services.AddScoped<ICheckData, CheckData>();

            services.AddScoped<IInvoker, Invoker>();

            services.AddScoped<IRunner, Runner>();

            services.AddScoped<IIntLinkedListRepository, IntLinkedListRepository>();

            return services;
        }
    }
}
