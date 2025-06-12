using Microsoft.AspNetCore.SignalR;
using Statio.Hubs;
using System.Diagnostics;
using System.Threading;

namespace Statio.Services
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IHubContext<PerformanceCounterHub> _performanceCounterHub;
        private readonly PerformanceCounter _cpuCounter;

        public Worker(ILogger<Worker> logger, IHubContext<PerformanceCounterHub> pcHub)
        {
            _logger = logger;
            _performanceCounterHub = pcHub;
            _cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {Time}", DateTime.Now);

                await _performanceCounterHub.Clients.All.SendAsync("StatusUpdate", _cpuCounter.NextValue());
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}