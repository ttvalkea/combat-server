using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class CombatHub : Hub
{
    public async Task BroadcastConnectionAmountData(int data) => await Clients.All.SendAsync("broadcastconnectionamountdata", data);
    public async Task BroadcastPlayerDataMessage(Player data) => await Clients.All.SendAsync("broadcastPlayerDataMessage", data);
    public async Task BroadcastFireballDataMessage(Fireball data) => await Clients.All.SendAsync("broadcastFireballDataMessage", data);
    public async Task BroadcastFireballHitPlayerMessage(Fireball fireball, Player player) => await Clients.All.SendAsync("broadcastFireballHitPlayerMessage", new FireballHitPlayerData(fireball, player));
    public async Task BroadcastGetObstacles(bool generateNewObstacles) => await Clients.All.SendAsync("broadcastGetObstacles", GetObstacles(generateNewObstacles));
    

    public override Task OnConnectedAsync()
    {
        PersistingValues.NumberOfPlayers++;
        Console.WriteLine(PersistingValues.NumberOfPlayers);
        BroadcastConnectionAmountData(PersistingValues.NumberOfPlayers);
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception exception)
    {
        PersistingValues.NumberOfPlayers--;
        Console.WriteLine(PersistingValues.NumberOfPlayers);
        BroadcastConnectionAmountData(PersistingValues.NumberOfPlayers);
        return base.OnDisconnectedAsync(exception);
    }

    public List<Obstacle> GetObstacles(bool generateNewObstacles)
    {
        if (generateNewObstacles)
        {
            var rng = new Random();
            var amount = rng.Next(3, 7);
            var obstacles = new List<Obstacle>();
            for (var i = 0; i < amount; i++)
            {
                obstacles.Add(new Obstacle() { positionX = rng.Next(0, 50), positionY = rng.Next(0, 50), sizeX = rng.Next(3, 13), sizeY = rng.Next(3, 13), id = Utils.GetId() });
            }
            PersistingValues.Obstacles = obstacles;
        }

        return PersistingValues.Obstacles;
    }
}
