public class FireballHitPlayerData
{
    public string playerId { get; set; }
    public string fireballId { get; set; }

    //public FireballHitPlayerData(Fireball fireball, Player player)
    //{
    //    fireballId = fireball.id;
    //    playerId = player.id;
    //}

    public FireballHitPlayerData(string idOfFireball, string idOfPlayer)
    {
        fireballId = idOfFireball;
        playerId = idOfPlayer;
    }
}