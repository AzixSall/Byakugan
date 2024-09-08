using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkerService.Repositories;
using Microsoft.AspNetCore.SignalR;
using Shared.Models;

namespace WorkerService.Data
{
    public class LocationDataHub : Hub
    {
        public async Task SendLocationUpdate(IpDetails details)
        {
            await Clients.All.SendAsync("ReceiveNewLocation", details);
        }
    }
}
