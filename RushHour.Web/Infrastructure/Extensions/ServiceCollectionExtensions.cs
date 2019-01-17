namespace RushHour.Web.Infrastructure.Extensions
{
    using Microsoft.Extensions.DependencyInjection;
    using Data.Contracts;
    using Data.Models;
    using Service.Contracts;
    using System.Linq;
    using System.Reflection;

    public static class ServiceCollcetionExtensions
    {
        public static IServiceCollection AddDomainServices(this IServiceCollection services)
        {
            Assembly dataAssembly = Assembly.GetAssembly(typeof(IRepository<User>));
            Assembly serviceAssembly = Assembly.GetAssembly(typeof(IService<User>));

            RegisterServices(services, dataAssembly);
            RegisterServices(services, serviceAssembly);

            return services;
        }

        private static void RegisterServices(IServiceCollection services, Assembly assembly)
        {
            assembly
                .GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.GetInterfaces().Any(i => i.Name == $"I{t.Name}"))
                .Select(t => new
                {
                    Inteface = t.GetInterface($"I{t.Name}"),
                    Implementation = t
                })
                .ToList()
                .ForEach(s => services.AddTransient(s.Inteface, s.Implementation));
        }
    }
}
