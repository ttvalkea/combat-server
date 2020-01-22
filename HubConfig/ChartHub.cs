using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Threading.Tasks;

public class ChartHub : Hub
{
    public async Task BroadcastChartData(string data) => await Clients.All.SendAsync("broadcastchartdata", data);
}
