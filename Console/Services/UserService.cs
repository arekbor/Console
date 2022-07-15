using System.Security.Claims;
using AutoMapper;
using Console.Contracts;
using Console.Data;
using Console.ModelDto;
using Console.Models;
using Console.Persistence;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Console.Services;
public class UserService:IUserService
{
    private readonly AppDbContext _appDbContext;
    private readonly IActionContextAccessor _contextAccessor;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserContextService _userContextService;
    public UserService(
        AppDbContext appDbContext, 
        IActionContextAccessor contextAccessor, 
        IPasswordHasher<User> passwordHasher, 
        IMapper mapper,
        IConfiguration configuration, 
        IUserContextService userContextService,
        IHttpContextAccessor httpContextAccessor)
    {
        _appDbContext = appDbContext;
        _contextAccessor = contextAccessor;
        _passwordHasher = passwordHasher;
        _mapper = mapper;
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;
        _userContextService = userContextService;
    }

    public async Task<User> LoginAsync(LoginUserDto model)
    {
        var user = await _appDbContext
            .DbUserContext
            .FirstOrDefaultAsync(x => x.Username.Equals(model.Username));
        if (user is null)
        {
            _contextAccessor.ActionContext?.ModelState.AddModelError("",Message.Code05);
            return null;
        }

        if (_passwordHasher.VerifyHashedPassword(user, user.Password, model.Password) ==
            PasswordVerificationResult.Failed)
        {
            _contextAccessor.ActionContext?.ModelState.AddModelError("", Message.Code05);
            return null;
        }
        
        await SetUserPrincipalAsync(user);
        
        _contextAccessor.ActionContext?.ModelState.Clear();
        return user;
    }

    public async Task LogoutAsync()
    {
        if (_httpContextAccessor.HttpContext != null)
            await _httpContextAccessor.HttpContext.SignOutAsync(_configuration["CookieAuthenticationName"]);
    }

    public async Task<User> RegisterAsync(RegisterUserDto model)
    {
        if (await _appDbContext.DbUserContext.AnyAsync(
                x => x.Username.Equals(model.Username)))
        {
            _contextAccessor.ActionContext?.ModelState.AddModelError("",Message.Code02);
            return null;
        }

        if (!model.Password.Equals(model.ConfirmPassword))
        {
            _contextAccessor.ActionContext?.ModelState.AddModelError("",Message.Code03);
            return null;
        }

        if (model.Password.Equals(model.Username))
        {
            _contextAccessor.ActionContext?.ModelState.AddModelError("",Message.Code04);
            return null;
        }

        var user = _mapper.Map<User>(model);

        var pwdHasher = _passwordHasher.HashPassword(user, model.Password);
        user.Password = pwdHasher;
        
        _contextAccessor.ActionContext?.ModelState.Clear();
        return user;
    }
    public async Task Add(User user)
    {
        await _appDbContext.DbUserContext.AddAsync(user);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task<User> GetUserById(int id)
    {
        var user = await _appDbContext
            .DbUserContext.FirstOrDefaultAsync(x => x.Id == id);
        return user;
    }

    private async Task SetUserPrincipalAsync(User user)
    {
        System.Console.WriteLine(user);
        var claims = new List<Claim>()
        {
            new(ClaimTypes.NameIdentifier,user.Id.ToString()),
            new(ClaimTypes.Name, user.Username),
        };
        var claimsIdentity = new ClaimsIdentity(claims, _configuration["CookieAuthenticationName"]);
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
        
        var authenticationProperties = new AuthenticationProperties
        {
            IsPersistent = true
        };

        if (_httpContextAccessor.HttpContext != null)
            await _httpContextAccessor.HttpContext.SignInAsync(claimsPrincipal, authenticationProperties);
    }
}