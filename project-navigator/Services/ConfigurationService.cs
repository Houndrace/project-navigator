using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;

namespace project_navigator.services;

public interface IConfigurationService
{
    string? GetConnectionString();
    void SetConnectionString(string connection);
}

public class ConfigurationService : IConfigurationService
{
    private const string ConfigFilePath = "config.json";
    private const string ConnectionStringsSection = "ConnectionStrings";
    private const string MainConnectionSection = "DefaultDbConnection";
    private JObject _configuration;

    public ConfigurationService()
    {
        LoadConfig();
    }

    public string? GetConnectionString()
    {
        return _configuration[ConnectionStringsSection]?[MainConnectionSection]?.Value<string>();
    }

    public void SetConnectionString(string connection)
    {
        ArgumentException.ThrowIfNullOrEmpty(connection);

        _configuration[ConnectionStringsSection] ??= new JObject();
        _configuration[ConnectionStringsSection]![MainConnectionSection] = connection;
        SaveConfig();
    }

    private void SaveConfig()
    {
        using var file = File.CreateText(ConfigFilePath);
        using var writer = new JsonTextWriter(file);
        writer.Formatting = Formatting.Indented;
        _configuration.WriteTo(writer);
    }

    private void LoadConfig()
    {
        try
        {
            using var jsonReader = new JsonTextReader(File.OpenText(ConfigFilePath));
            _configuration = JObject.Load(jsonReader);
        }
        catch (Exception e)
        {
            Log.Warning(e, "Ошибка загрузки конфигурации");
            _configuration = CreateConfig();
            SaveConfig();
        }
    }

    private JObject CreateConfig(string connectionString = "YourConnectionStringHere")
    {
        var config = new JObject
        {
            [ConnectionStringsSection] = new JObject
            {
                [MainConnectionSection] = connectionString
            }
        };
        return config;
    }
}