using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
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
            //.UseWPFAppLifetime()
            .Build();
        }

        private void OnApplicationExit(object sender, ExitEventArgs e)
        {
            _host.Services.GetRequiredService<MainWindow>().Close();

            var frame = new DispatcherFrame(false);

            Task.Run(async () => {
                try
                {
                    await _host.StopAsync();
                }
                finally
                {
                    frame.Continue = false;
                }
            });

            Dispatcher.PushFrame(frame);

            _host.Dispose();

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
