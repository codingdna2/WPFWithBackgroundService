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
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(10, stoppingToken);
            }

            Debug.WriteLine("MyService is stopping..");

            await Task.Delay(100, stoppingToken);

            Debug.WriteLine("MyService is stopped..");
        }
    }
}
