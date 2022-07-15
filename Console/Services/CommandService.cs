using System.Security.Claims;
using System.Text.RegularExpressions;
using Console.Contracts;
using Console.Data;
using Console.Hubs;
using Console.ModelDto;
using Console.Models;
using Microsoft.AspNetCore.SignalR;

namespace Console.Services;

public class CommandService:ICommandService
{
    private readonly IUserService _userService;
    private readonly Dictionary<string, Delegate> _commands;
    private readonly IConfiguration _configuration;
    private readonly IHubService _hubService;
    private readonly IUserContextService _userContextService;

    public CommandService(
        IUserService userService, 
        IConfiguration configuration, 
        IHubService hubService,
        IUserContextService userContextService)
    {
        _userService = userService;
        _configuration = configuration;
        _hubService = hubService;
        _userContextService = userContextService;
        _commands = new Dictionary<string, Delegate>
        {
            { "!help", CmdHelp },
            { "!exit", CmdLogout },
            { "!who", CmdWho },
        };
    }
    public bool IsCommand(string text)
    {
        return text.StartsWith(ComanndsSetting.CommandSubstring);
    }

    public void Command(string userId, string userName, string text)
    {
        var words = text.Split();
        
        System.Console.WriteLine($"user ({userId}) {userName} used cmd {words.First()}");
        var value = _commands
            .FirstOrDefault(x => x.Key.Equals(words.First())).Value;
        if (value is null)
            return;
        
        System.Console.WriteLine($"command used to delegate {value}");
        _commands[words.First()].DynamicInvoke(userId, userName, text, words.Last());
    }
    private async Task CmdLogout(string userId = "", string userName = "", string text = "", string par = "")
    {
        var content = new Content()
        {
            UserId = userId,
            Text = "logout",
            Username = userName,
            TimeStamp = DateTime.Now,
        };
        await _hubService.SendTextAsync(content, _configuration["MainRoomName"]);
        
        await _userService.LogoutAsync();
    }

    private async Task CmdHelp(string userId = "", string userName = "", string text = "", string par = "")
    {
        var listOfCommands = _commands.Keys.ToList();
        var content = new Content()
        {
            UserId = userId,
            Text = string.Join(", ",listOfCommands),
            Username = userName,
            TimeStamp = DateTime.Now,
        };
        await _hubService.SendTextClientAsync(content, _userContextService.GetUserId().ToString());
    }

    private async Task CmdWho(string userId = "", string userName = "", string text = "", string par = "")
    {
        var content = new Content()
        {
            UserId = userId,
            Text = $"{userName} {userId}",
            Username = userName,
            TimeStamp = DateTime.Now,
        };
        await _hubService.SendTextClientAsync(content, _userContextService.GetUserId().ToString());
    }
}