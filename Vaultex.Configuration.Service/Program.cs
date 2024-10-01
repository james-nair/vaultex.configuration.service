using Microsoft.EntityFrameworkCore;
using Vaultex.Configuration;
using Vaultex.Database.Extensions;
using Vaultex.Database.Options;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddOptions();
builder.Services.Configure<DatabaseOptions>(builder.Configuration.GetSection(nameof(DatabaseOptions)));

builder.Services.RegisterDbContext<ConfigContext>(options => options.UseNpgsql(DBConnection.ConnectionString("vt_configuration", builder.Configuration).ConnectionString));


var host = builder.Build();
using (var scope = host.Services.CreateScope())
{
    var logger = scope.ServiceProvider.GetService<ILoggerFactory>();
    var log = logger.CreateLogger("ConfigServiceStartup");
    log.LogInformation("PreMigrate ConfigurationDB");
    scope.ServiceProvider.CreateAndMigrate<ConfigContext>();
    log.LogInformation("Migration complete!");
}

host.Run();