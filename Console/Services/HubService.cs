using Console.Contracts;
using Console.Hubs;
using Console.Models;
using Microsoft.AspNetCore.SignalR;

namespace Console.Services;

public class HubService:IHubService
{
    private readonly IHubContext<ConsoleHub> _hubContext;
    public HubService(IHubContext<ConsoleHub> hubContext)
    {
        _hubContext = hubContext;
    }
    public async Task AddToGroupAsync(string connectionId, string roomName)
    {
        await _hubContext.Groups.AddToGroupAsync(connectionId, roomName);
        System.Console.WriteLine($"ConnectionId: {connectionId} added to {roomName} group");
    }

    public async Task RemoveFromGroupAsync(string connectionId, string roomName)
    {
        await _hubContext.Groups.RemoveFromGroupAsync(connectionId,roomName);
        System.Console.WriteLine($"ConnectionId: {connectionId} removed from {roomName} group");
    }

    public async Task SendTextAsync(Content content, string roomName)
    {
        await _hubContext.Clients.Group(roomName)
            .SendAsync("RecieveTerminal", new
            {
                UserId = $"[{content.UserId}]",
                Text = content.Text,
                Username = content.Username,
                TimeStamp = $"[{content.TimeStamp:T}]",
            });
        System.Console.WriteLine($"name: {content.Username} date: {content.TimeStamp.ToString("t")} text: {content.Text}");
    }

    public async Task SendTextClientAsync(Content content, string userId)
    {
        await _hubContext.Clients.User(userId)
            .SendAsync("RecieveTerminal",new
            {
                UserId = $"[{content.UserId}]",
                Text = content.Text,
                Username = content.Username,
                TimeStamp = $"[{content.TimeStamp:T}]",
            });
        System.Console.WriteLine($"(Client only) name: {content.Username} date: {content.TimeStamp.ToString("t")} text: {content.Text}");
    }
}