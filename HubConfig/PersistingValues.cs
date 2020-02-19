
using System.Collections.Generic;

public static class PersistingValues
{
    static PersistingValues() {
        Obstacles = new List<Obstacle>();
    }

    public static List<Obstacle> Obstacles { get; set; }
    public static int NumberOfPlayers { get; set; }
}


