using Microsoft.AspNetCore.SignalR;
using Razor_WebAPI_jQuery_AJAX.Models;
using System.Threading.Tasks;

namespace Razor_WebAPI_Application.Hubs
{
    public class ChartHub : Hub
    {
        public async Task UpdateChart(ChartData chartData)
        {
            await Clients.All.SendAsync("UpdateChart", chartData);
        }
    }
}