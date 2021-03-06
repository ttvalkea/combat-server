using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

public class CombatHub : Hub
{
    public async Task BroadcastPlayerWins(Player player) => await Clients.All.SendAsync("broadcastPlayerWins", player);
    public async Task BroadcastGetTagPlayerId() => await Clients.All.SendAsync("broadcastGetTagPlayerId", PersistingValues.TagPlayerId);
    public async Task BroadcastConnectionAmountData(int data) => await Clients.All.SendAsync("broadcastconnectionamountdata", data);
    public async Task BroadcastPlayerDataMessage(Player data) => await Clients.All.SendAsync("broadcastPlayerDataMessage", data);
    public async Task BroadcastFireballDataMessage(Fireball data) => await Clients.All.SendAsync("broadcastFireballDataMessage", data);
    public async Task BroadcastFireballHitPlayerMessage(Fireball fireball, Player player)
    {
        await Clients.All.SendAsync("broadcastFireballHitPlayerMessage", new FireballHitPlayerData(fireball.id, player.id));

        //The player who knocks out the tag player becomes the tag.
        if (player.hitPoints <= 1 && player.id == PersistingValues.TagPlayerId)
        {
            await BroadcastPlayerBecomesTag(fireball.casterId);
        }
    }
    public async Task BroadcastGetObstacles(bool generateNewObstacles) => await Clients.All.SendAsync("broadcastGetObstacles", GameMechanics.GetObstacles(generateNewObstacles));
    public async Task BroadcastPlayerHitNewTagItem(string playerId) {
        await BroadcastPlayerBecomesTag(playerId);
        PersistingValues.TagItem = new NewTagItem(0, 0, false);
        await BroadcastNewTagItemData();
    }
    public async Task BroadcastPlayerBecomesTag(string playerId) {
        PersistingValues.TagPlayerId = playerId;
        await Clients.All.SendAsync("broadcastPlayerBecomesTag", playerId);
    }
    public async Task BroadcastNewTagItemData() => await Clients.All.SendAsync("newTag", PersistingValues.TagItem);   
    public override Task OnConnectedAsync()
    {
        PersistingValues.IdsOfConnectedClients.Add(Context.ConnectionId);
        BroadcastConnectionAmountData(PersistingValues.IdsOfConnectedClients.Count);
        return base.OnConnectedAsync();
    }
    public override Task OnDisconnectedAsync(Exception exception)
    {
        PersistingValues.IdsOfConnectedClients.Remove(Context.ConnectionId);
        BroadcastConnectionAmountData(PersistingValues.IdsOfConnectedClients.Count);
        return base.OnDisconnectedAsync(exception);
    }
}
