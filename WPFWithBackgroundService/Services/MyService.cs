using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WPFWithBackgroundService.Services
{
    public class MyService : HostedServiceBase
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    await Task.Delay(10, stoppingToken);
                }
            }
            catch (TaskCanceledException)
            {
                Debug.WriteLine("MyService is stopping..");
                Thread.Sleep(10);
            }

            Debug.WriteLine("MyService is stopped..");
        }
    }
}
