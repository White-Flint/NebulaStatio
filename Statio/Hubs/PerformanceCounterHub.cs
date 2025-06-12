using Microsoft.AspNetCore.SignalR;

namespace Statio.Hubs
{
    public class PerformanceCounterHub : Hub
    {
        public async Task UpdateStatus(double utilisation)
        {
            await Clients.All.SendAsync("StatusUpdate", utilisation);
        }
    }
}
