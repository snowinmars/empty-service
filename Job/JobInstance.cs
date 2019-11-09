using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Job
{
    // ReSharper disable once AllowPublicClass
    public class JobInstance : IHostedService, IDisposable
    {
        private Timer timer;

        public void Dispose()
        {
            timer?.Dispose();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await Console.Out.WriteLineAsync("Started");

            timer = new Timer(Work, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Console.Out.WriteLineAsync("Stopped");
            timer.Change(Timeout.Infinite, 0);
        }

        private void Work(object unused)
        {
            Console.WriteLine("Tick");
        }
    }
}