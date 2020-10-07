using Newtonsoft.Json;
using Serilog;
using System;
using System.IO;

class Program
{
    static AppSettings GetAppSettings(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("appsettings.json was not found or is not accesible");
        }

        var appSettingsContent = File.ReadAllText(filePath);
        if (string.IsNullOrEmpty(appSettingsContent))
            throw new Exception("Appsettings file is empty");

        return JsonConvert.DeserializeObject<AppSettings>(appSettingsContent);
    }

    static void Main()
    {
        try
        {
            var appSettingsFilePath = "../../../appsettings.json";
            var appSettings = GetAppSettings(appSettingsFilePath);

            Log.Logger = new LoggerConfiguration()
                                .MinimumLevel.Information()
                                .WriteTo.File(appSettings.LogPath, rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true)
                                .CreateLogger();

            Log.Information($"Starting EPLAST Activity Checker");
            var dbService = new DbService(appSettings.DbConnection);
            dbService.PublishScheduledArticles(item =>
            {
                Log.Information(item);
            });

            Log.Information($"ActivityChecker finished. Closing & flushing logs...");
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"App failed with error: {ex.Message}");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}