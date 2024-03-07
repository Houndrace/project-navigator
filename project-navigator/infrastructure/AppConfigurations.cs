using System.IO;
using Microsoft.Extensions.Configuration;

namespace project_navigator.infrastructure;

public class AppConfigurations
{
    public static IConfigurationRoot GetEnvConfig()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("env.json")
            .Build();


        return configuration;
    }
}