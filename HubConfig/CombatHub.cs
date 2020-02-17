using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class CombatHub : Hub
{
    public async Task BroadcastChatMessage(string data) => await Clients.All.SendAsync("broadcastchatmessage", data);
    public async Task BroadcastConnectionAmountData(int data) => await Clients.All.SendAsync("broadcastconnectionamountdata", data);


    public async Task BroadcastPlayerDataMessage(Player data) => await Clients.All.SendAsync("broadcastPlayerDataMessage", data);
    public async Task BroadcastFireballDataMessage(Fireball data) => await Clients.All.SendAsync("broadcastFireballDataMessage", data);
    public async Task BroadcastFireballHitPlayerMessage(Fireball fireball, Player player) => await Clients.All.SendAsync("broadcastFireballHitPlayerMessage", new FireballHitPlayerData(fireball, player));


    public override Task OnConnectedAsync()
    {
        ConnectionCount.number++;
        Console.WriteLine(ConnectionCount.number);
        BroadcastConnectionAmountData(ConnectionCount.number);
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception exception)
    {
        ConnectionCount.number--;
        Console.WriteLine(ConnectionCount.number);
        BroadcastConnectionAmountData(ConnectionCount.number);
        return base.OnDisconnectedAsync(exception);
    }
}

public static class ConnectionCount
{
    public static int number = 0;
}