using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace project_navigator.Helpers;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTransientFromNamespace(
        this IServiceCollection services,
        Assembly assembly,
        string namespaceName
    )
    {
        var types = assembly
            .GetTypes()
            .Where(x =>
                x.IsClass
                && !x.IsAbstract
                && x.Namespace != null
                && x.Namespace!.StartsWith(namespaceName, StringComparison.InvariantCultureIgnoreCase)
            );

        foreach (var type in types)
            if (services.All(x => x.ServiceType != type))
                _ = services.AddTransient(type);


        return services;
    }
}