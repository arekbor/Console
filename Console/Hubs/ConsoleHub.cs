using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Console.Hubs;

[Authorize]
public class ConsoleHub:Hub
{
    public string GetConnectionId() => Context.ConnectionId;
}