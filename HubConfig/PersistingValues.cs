
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;

public static class PersistingValues
{
    static PersistingValues() {
        Obstacles = new List<Obstacle>();
        StartNewTagInfoSentThisCycleResetTimer();
        NewTagInfoSentThisCycle = false;
        StartNewTagItemTimer();

        //Start this timer half a second later than the resetting timer. This way, NewTagInfoSentThisCycle should always be false for the first attempt of NewTagItemTimer.Elapsed event.
        Task.Delay(500).ContinueWith(t => EnableNewTagItemTimer());
    }

    public static Timer NewTagItemTimer { get; set; }
    public static Timer NewTagInfoSentThisCycleResetTimer { get; set; }
    public static bool NewTagInfoSentThisCycle { get; set; }
    public static List<Obstacle> Obstacles { get; set; }
    public static int NumberOfPlayers { get; set; }


    //Set up loops
    private static void StartNewTagItemTimer()
    {
        NewTagItemTimer = new Timer(Constants.NEW_TAG_INTERVAL_MS);
        NewTagItemTimer.AutoReset = true;
        NewTagItemTimer.Enabled = false;
    }
    private static void EnableNewTagItemTimer()
    {
        NewTagItemTimer.Enabled = true;
    }
    private static void StartNewTagInfoSentThisCycleResetTimer()
    {
        NewTagInfoSentThisCycleResetTimer = new Timer(Constants.NEW_TAG_INTERVAL_MS);
        NewTagInfoSentThisCycleResetTimer.AutoReset = true;
        NewTagInfoSentThisCycleResetTimer.Elapsed += OnNewTagInfoSentThisCycleResetTimerEvent;
        NewTagInfoSentThisCycleResetTimer.Enabled = true;
    }
    private static void OnNewTagInfoSentThisCycleResetTimerEvent(Object source, ElapsedEventArgs e)
    {
        NewTagInfoSentThisCycle = false;
    }
}


