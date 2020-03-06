using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;
using System.Timers;

[Route("api/[controller]")]
[ApiController]
public class HubController : ControllerBase
{
    public Timer GameTimer { get; set; }
    private IHubContext<CombatHub> _hub;

    public HubController(IHubContext<CombatHub> hub)
    {
        _hub = hub;
        SetTimer();
    }

    public IActionResult Get()
    {
        return Ok(new { Message = "Request Completed" });
    }

    //Set up a loop
    private void SetTimer()
    {
        PersistingValues.NewTagItemTimer.Elapsed += OnNewTagItemTimerEvent;
    }

    //Send data to all clients periodically
    private async void OnNewTagItemTimerEvent(Object source, ElapsedEventArgs e)
    {
        //Only sent once every ten seconds no matter the number of clients.
        if (PersistingValues.IdsOfConnectedClients.Count > 0 && !PersistingValues.NewTagInfoSentThisCycle)
        {
            PersistingValues.NewTagInfoSentThisCycle = true;
            await SendNewTagPositionToAllClients();
        }
    }

    private async Task SendNewTagPositionToAllClients()
    {
        PersistingValues.TagItem = GameMechanics.GetNewTagItem();
        await _hub.Clients.All.SendAsync("newTag", PersistingValues.TagItem); 
    }    
}