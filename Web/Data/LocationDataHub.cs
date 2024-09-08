using Microsoft.AspNetCore.SignalR;
using Shared.Models;

namespace Web.Data
{
    public class LocationDataHub : Hub
    {
        public async Task SendLocationUpdate(IpDetails details)
        {
            await Clients.All.SendAsync("ReceiveNewLocation", details);
        }
    }
}
