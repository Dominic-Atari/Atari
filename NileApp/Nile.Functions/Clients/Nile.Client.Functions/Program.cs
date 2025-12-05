using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureLogging(logging =>
    {
        logging.ClearProviders();
        logging.AddConsole();
    })
    .ConfigureServices(services =>
    {
        // Register shared services here (validators, security, mapper, accessors, etc.)
        // services.AddSingleton<IMapper, Nile.Accessors.Mapper>();
        // services.AddScoped<IUserAccessor, Nile.Accessors.User.UserAccessor>();
        // services.AddSingleton<IDateUtility, Nile.Utilities.DateUtility>();
        // services.AddSingleton<IIdentityContext, IdentityContext>();
        // services.AddSingleton<IAuthorizationService, AuthorizationService>();
    })
    .Build();

host.Run();
