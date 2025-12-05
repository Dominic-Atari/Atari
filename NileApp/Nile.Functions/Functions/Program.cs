using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Azure.Functions.Worker.ApplicationInsights;
using Nile.Common.Extensions;
using Nile.Utilities.AzureSdk;
using Nile.Accessors;
using Nile.Managers.Proxys;

// Build and run isolated Functions host
var host = new HostBuilder()
    .ConfigureAppConfiguration((context, config) =>
    {
        var env = context.HostingEnvironment.EnvironmentName;
        config
            .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();
    })
    .ConfigureFunctionsWebApplication()
    .ConfigureServices((context, services) =>
    {
        // Observability: Application Insights for isolated worker
        services.ConfigureFunctionsApplicationInsights();

        // Configuration & utilities
        services.AddSingleton<IConfiguration>(sp => context.Configuration);
        services.AddSingleton<ConfigUtility>();                       // concrete
        services.AddSingleton<IConfigUtility>(sp => sp.GetRequiredService<ConfigUtility>()); // interface -> concrete
        services.AddSingleton<IDateUtility, DateUtility>();   // use the shared DateUtility

        // Core media services
        services.AddSingleton<IBlobStorageUtility, AzureSdkUtility>();
        services.AddSingleton<IVideoContentService, AzureSdkUtility>();
        services.AddSingleton<IPhotoContentService, AzureSdkUtility>();

        // Generic manager proxy available for any TManager
        services.AddSingleton(typeof(IProxy<>), typeof(Proxy<>));
    })
    .Build();

host.Run();