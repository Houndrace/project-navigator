using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using project_navigator.views.windows;
using Serilog;
using Wpf.Ui;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace project_navigator.services;

public interface IConfigurationService
{
    string? GetConnectionString();
    void SetConnectionString(string connection);
}

public class ConfigurationService : IConfigurationService
{
    private const string ConfigFilePath = "config.json";
    public const string SystemTheme = "System";
    public const string LightTheme = "Light";
    public const string DarkTheme = "Dark";
    private const string ConnectionStringsSection = "ConnectionStrings";
    private const string MainConnectionSection = "DefaultDbConnection";
    private const string ThemePreferenceSection = "Theme";

    private readonly JObject _configuration;

    public ConfigurationService()
    {
        var loadConfigAttempt = LoadConfig();

        if (loadConfigAttempt == null)
        {
            _configuration = CreateConfig();
            SaveConfig();
        }
        else
        {
            _configuration = loadConfigAttempt;
        }
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

    private JObject? LoadConfig()
    {
        try
        {
            using var jsonReader = new JsonTextReader(File.OpenText(ConfigFilePath));
            return JObject.Load(jsonReader);
        }
        catch (Exception e)
        {
            Log.Warning(e, "Ошибка загрузки конфигурации");
            return null;
        }
    }

    private JObject CreateConfig(string connectionString = "YourConnectionStringHere")
    {
        return new JObject
        {
            [ConnectionStringsSection] = new JObject
            {
                [MainConnectionSection] = connectionString
            },
            [ThemePreferenceSection] = "Light"
        };
    }
}