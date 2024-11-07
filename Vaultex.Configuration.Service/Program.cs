using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Vaultex.Configuration;
using Vaultex.Configuration.Client;
using Vaultex.Configuration.Service.Jobs;
using Vaultex.Database.Extensions;
using Vaultex.Database.Options;
using Vaultex.Service.Shared.Hangfire;
using Vaultex.Service.Shared.Services;
using Vaultex.Settings;
using Vaultex.Settings.Definitions;
using Vaultex.Settings.Interfaces;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddOptions();
builder.Services.Configure<DatabaseOptions>(builder.Configuration.GetSection(nameof(DatabaseOptions)));

builder.Services.RegisterDbContext<ConfigContext>(options => options.UseNpgsql(DBConnection.ConnectionString("vt_configuration", builder.Configuration).ConnectionString));

//Dependency injections

builder.Services.AddSingleton<ISettingsClient, SettingsClient>();
builder.Services.AddSingleton<ISettingCreator, SettingCreator>();

builder.Services.RegisterJob<TestJob, TestJobSetting>();
//builder.Services.AddSingleton<IJobScheduler, HangfireJobScheduler>();
builder.Services.AddHangfireSetup();

builder.Services.AddHostedService<StartableHostedService>();


var host = builder.Build();

using (var scope = host.Services.CreateScope())
{
    var logger = scope.ServiceProvider.GetService<ILoggerFactory>();
    var log = logger.CreateLogger("ConfigServiceStartup");
    log.LogInformation("PreMigrate ConfigurationDB");
    scope.ServiceProvider.CreateAndMigrate<ConfigContext>();
    log.LogInformation("Migration complete!");

    var settingClient = scope.ServiceProvider.GetRequiredService<ISettingsClient>();
    //Configure settings - Look through all settings, and populate required ones
    var requiredSettings = SettingCreator.SettingTypes.Where(s => s.GetCustomAttribute<SettingAttribute>()?.IsRequired == true);
    List<Setting> settingsToAdd = [];
    foreach (var type in requiredSettings)
    {
        var settingList = await settingClient.GetSettingsByType<Setting>(type);
        var isSet = settingList.FirstOrDefault();
        if (isSet == null)
        {
            //Add a default setting
            var newSetting = SettingCreator.CreateSettingFromType(type);
            settingsToAdd.Add(newSetting);
            log.LogInformation($"Added Default Setting to Db {type.Name}");
        }
    }
    if (settingsToAdd.Count != 0) await settingClient.BulkSaveSettings(settingsToAdd);

}

host.Run();