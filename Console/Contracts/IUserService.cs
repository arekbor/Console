using Console.ModelDto;
using Console.Models;

namespace Console.Contracts;

public interface IUserService
{
    Task<User> LoginAsync(LoginUserDto model);
    Task LogoutAsync();
    Task<User> RegisterAsync(RegisterUserDto model);
    Task Add(User user);
    Task<User> GetUserById(int id);
}