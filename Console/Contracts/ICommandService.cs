namespace Console.Contracts;

public interface ICommandService
{
    public bool IsCommand(string text);

    public void Command(string userId, string userName, string text);
}