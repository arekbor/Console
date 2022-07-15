using Console.Models;

namespace Console.Contracts;

public interface IHubService
{
    Task AddToGroupAsync(string connectionId, string roomName);
    Task RemoveFromGroupAsync(string connectionId, string roomName);
    Task SendTextAsync(Content content, string roomName);
    Task SendTextClientAsync(Content content, string userId);
}