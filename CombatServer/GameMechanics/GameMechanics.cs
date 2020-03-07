using System;
using System.Collections.Generic;

public static class GameMechanics
{
    public static List<Obstacle> GetObstacles(bool generateNewObstacles)
    {
        if (generateNewObstacles)
        {
            var rng = new Random();
            var amount = rng.Next(Constants.OBSTACLE_AMOUNT_MIN, Constants.OBSTACLE_AMOUNT_MAX);
            var obstacles = new List<Obstacle>();
            for (var i = 0; i < amount; i++)
            {
                var obstacleSizeX = rng.Next(Constants.OBSTACLE_SIZE_MIN, Constants.OBSTACLE_SIZE_MAX);
                var obstacleSizeY = rng.Next(Constants.OBSTACLE_SIZE_MIN, Constants.OBSTACLE_SIZE_MAX);
                obstacles.Add(new Obstacle()
                {
                    positionX = rng.Next(0, Constants.PLAY_AREA_SIZE_X - obstacleSizeX),
                    positionY = rng.Next(0, Constants.PLAY_AREA_SIZE_Y - obstacleSizeY),
                    sizeX = obstacleSizeX,
                    sizeY = obstacleSizeY,
                    id = Utils.GetId()
                });
            }
            PersistingValues.Obstacles = obstacles;
        }

        return PersistingValues.Obstacles;
    }

    public static NewTagItem GetNewTagItem()
    {
        var rng = new Random();
        var x = rng.Next(1, Constants.PLAY_AREA_SIZE_X - 1);
        var y = rng.Next(1, Constants.PLAY_AREA_SIZE_Y - 1);
        return new NewTagItem(x, y, true);
    }
}


