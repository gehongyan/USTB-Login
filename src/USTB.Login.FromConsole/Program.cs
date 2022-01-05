namespace USTB.Login.FromConsole;

internal static class Program
{
    private static async Task Main(string[] args)
    {
        string appSettingsFilePath = string.Empty;
        if (args.Length == 2 && args[0] == "-c")
        {
            appSettingsFilePath = args[1];
        }
        
        using IHost host = CreateHostBuilder(args, appSettingsFilePath).Build();
        await host.RunAsync();
    }

    /// <summary>
    ///     创建主机
    /// </summary>
    private static IHostBuilder CreateHostBuilder(string[] args, string appSettingsFilePath)
    {
        IHostBuilder hostBuilder = Host.CreateDefaultBuilder(args)
            .UseSystemd()
            .ConfigureServices((context, collection) => ConfigureServices(context, collection, appSettingsFilePath))
            .UseSerilog((hostingContext, services, loggerConfiguration) =>
            {
                loggerConfiguration
                    .ReadFrom.Configuration(hostingContext.Configuration)
                    .Enrich.FromLogContext()
                    .WriteTo.Console();
            });
        return hostBuilder;
    }

    /// <summary>
    ///     配置服务
    /// </summary>
    private static void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services, string appSettingsFilePath)
    {
        appSettingsFilePath = string.IsNullOrWhiteSpace(appSettingsFilePath)
            ? "appsettings.json"
            : appSettingsFilePath;
        
        IConfigurationRoot configurationRoot = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(appSettingsFilePath, false)
            .AddJsonFile($"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json", true, true)
            .Build();

        // USTB 登录模块配置
        USTBLoginConfiguration ustbLoginConfiguration = new();
        configurationRoot.GetSection(nameof(USTBLoginConfiguration))
            .Bind(ustbLoginConfiguration);

        services
            // 静态配置
            .AddSingleton(ustbLoginConfiguration)

            // 辅助程序
            .AddSingleton<UserAccountModule>()
            .AddSingleton<CheckInternetModule>()
            
            // 定时服务
            .AddHostedService<USTBAutoLoginService>();
    }
}