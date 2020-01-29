using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;

[Route("api/[controller]")]
[ApiController]
public class ChatController : ControllerBase
{
    private IHubContext<ChatHub> _hub;

    public ChatController(IHubContext<ChatHub> hub)
    {
        _hub = hub;
    }

    public IActionResult Get()
    {
        return Ok(new { Message = "Request Completed" });
    }
}