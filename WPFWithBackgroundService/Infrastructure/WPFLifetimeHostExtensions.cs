using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WPFWithBackgroundService
{
public static class WPFLifetimeHostExtensions
{
    public static IHostBuilder UseWPFAppLifetime(this IHostBuilder hostBuilder)
    {
        return hostBuilder.ConfigureServices((hostContext, services) => services.AddSingleton<IHostLifetime, WPFAppLifetime>());
    }
}
}
