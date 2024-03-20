using System.IO;
using Newtonsoft.Json.Linq;

namespace project_navigator.services;

public interface IConfigurationService
{
    bool IsConfigExists();
    void CreateConfig(string connectionString = "YourConnectionStringHere");
    string? GetConnectionString();
    void SetConnectionString(string connection);
}

//TODO:придумать логирование
public class ConfigurationService : IConfigurationService
{
    private const string ConfigFilePath = "config.json";
    private const string ConnectionStringsSection = "ConnectionStrings";
    private const string MainConnectionSection = "DefaultConnection";
    private JObject? _configuration;

    public ConfigurationService()
    {
        _configuration = LoadConfig();
    }

    public bool IsConfigExists()
    {
        return _configuration != null && _configuration.HasValues;
    }

    public string? GetConnectionString()
    {
        if (!IsConfigExists())
        {
            throw new InvalidOperationException("Configuration not found.");
        }

        return _configuration![ConnectionStringsSection]?[MainConnectionSection]?.Value<string>();
    }

    public void SetConnectionString(string connection)
    {
        ArgumentException.ThrowIfNullOrEmpty(connection);

        if (!IsConfigExists())
        {
            throw new InvalidOperationException("Configuration not found.");
        }

        _configuration![ConnectionStringsSection] ??= new JObject();
        _configuration[ConnectionStringsSection]![MainConnectionSection] = connection;
        SaveConfig(_configuration);
    }

    private JObject? LoadConfig()
    {
        try
        {
            var jsonContent = File.ReadAllText(ConfigFilePath);
            return JObject.Parse(jsonContent);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }

    public void CreateConfig(string connectionString = "YourConnectionStringHere")
    {
        var config = new JObject
        {
            [ConnectionStringsSection] = new JObject
            {
                [MainConnectionSection] = connectionString
            }
        };

        SaveConfig(config);
        _configuration = LoadConfig();
    }

    private void SaveConfig(JObject configFile)
    {
        File.WriteAllText(ConfigFilePath, configFile.ToString());
    }
}