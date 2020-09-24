using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using System.Windows;
using WPFWithBackgroundService.Services;

namespace WPFWithBackgroundService
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IHost _host;

        public static IHostBuilder CreateHostBuilder() => new HostBuilder().ConfigureAppConfiguration((context, configurationBuilder) =>
        {
            configurationBuilder.AddEnvironmentVariables();
            configurationBuilder.SetBasePath(context.HostingEnvironment.ContentRootPath);
        });

        public App()
        {
            _host = CreateHostBuilder().ConfigureServices((context, services) =>
            {
                services.AddHostedService<MyService>();
                services.AddSingleton<MainWindow>();
            })
            .UseWPFAppLifetime()
            .Build();
        }

        private async void OnApplicationExit(object sender, ExitEventArgs e)
        {
            using (_host)
            {
                _host.Services.GetRequiredService<MainWindow>().Close();

                await _host.StopAsync().ConfigureAwait(false);
            }

            _host = null;
        }

        private async void OnApplicationStartup(object sender, StartupEventArgs e)
        {
            await _host.StartAsync();

            var mainWindow = _host.Services.GetService<MainWindow>();
            mainWindow.Show();
        }
    }
}
