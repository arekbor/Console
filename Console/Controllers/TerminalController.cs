using System.Reflection;
using System.Security.Claims;
using Console.Contracts;
using Console.Data;
using Console.Hubs;
using Console.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Console.Controllers;

[Authorize]
[Route("[controller]")]
public class TerminalController : Controller
{
    
    private readonly IConfiguration _configuration;
    private readonly IUserContextService _userContextService;
    private readonly ICommandService _commandService;
    private readonly IHubService _hubService;
    public TerminalController(
        IConfiguration configuration, 
        IUserContextService userContextService, 
        IHubService hubService,
        ICommandService commandService)
    {
        
        _configuration = configuration;
        _userContextService = userContextService;
        _commandService = commandService;
        _hubService = hubService;
    }

    [HttpPost("[Action]/{connectionId}")]
    public async Task<IActionResult> JoinRoom(string connectionId)
    {
        await _hubService.AddToGroupAsync(connectionId, _configuration["MainRoomName"]);
        return Ok();
    }
    
    [HttpPost("[Action]/{connectionId}")]
    public async Task<IActionResult> LeaveRoom(string connectionId)
    {
        await _hubService.RemoveFromGroupAsync(connectionId, _configuration["MainRoomName"]);
        return Ok();
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Send(string text)
    {
        if (string.IsNullOrEmpty(text))
            return BadRequest();

        var content = new Content()
        {
            UserId = _userContextService.GetUserId().ToString(),
            Text = text,
            Username = _userContextService.GetUser().Identity?.Name,
            TimeStamp = DateTime.Now,
        };
        if (_commandService.IsCommand(content.Text))
        {
            _commandService.Command(content.UserId, content.Username, content.Text);
            return Ok(); 
        }

        await _hubService.SendTextAsync(content, _configuration["MainRoomName"]);
        return Ok();
    }
}