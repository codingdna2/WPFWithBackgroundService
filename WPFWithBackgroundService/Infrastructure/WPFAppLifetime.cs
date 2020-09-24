using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace WPFWithBackgroundService
{
    public class WPFAppLifetime : IHostLifetime, IDisposable
    {
        private readonly ManualResetEvent _shutdownBlock = new ManualResetEvent(false);

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task WaitForStartAsync(CancellationToken cancellationToken)
        {
            AppDomain.CurrentDomain.ProcessExit += OnProcessExit;

            return Task.CompletedTask;
        }

        private void OnProcessExit(object sender, EventArgs e)
        {
            if (!_shutdownBlock.WaitOne(new TimeSpan(0, 0, 5)))
            {
                Debug.WriteLine("Waiting for the host to be disposed. Ensure all 'IHost' instances are wrapped in 'using' blocks.");
            }
            _shutdownBlock.WaitOne();

            Environment.ExitCode = 0;
        }

        public void Dispose()
        {
            _shutdownBlock.Set();
            AppDomain.CurrentDomain.ProcessExit -= OnProcessExit;
        }
    }
}
