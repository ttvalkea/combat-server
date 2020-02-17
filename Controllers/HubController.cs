using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

[Route("api/[controller]")]
[ApiController]
public class HubController : ControllerBase
{
    private IHubContext<CombatHub> _hub;

    public HubController(IHubContext<CombatHub> hub)
    {
        _hub = hub;
    }

    public IActionResult Get()
    {
        return Ok(new { Message = "Request Completed" });
    }
}