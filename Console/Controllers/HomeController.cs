using System.Security.Claims;
using AutoMapper;
using Console.Contracts;
using Console.Hubs;
using Console.ModelDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Console.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly IUserService _userService;
    private readonly IUserContextService _userContextService;
    private readonly IMapper _mapper;
    public HomeController(IUserService userService, IUserContextService userContextService, IMapper mapper)
    {
        _userService = userService;
        _userContextService = userContextService;
        _mapper = mapper;
    }
    public async Task<IActionResult> Index()
    {
        var user = await _userService
            .GetUserById(_userContextService.GetUserId());
        return View(_mapper.Map<UserDto>(user));
    }
    [AllowAnonymous]
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }
    [AllowAnonymous]
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<IActionResult> Login(LoginUserDto loginUserDto)
    {
        if (!ModelState.IsValid)
            return View(loginUserDto);
        
        var user = await _userService.LoginAsync(loginUserDto);
        
        if (user is not null)
            return RedirectToAction("Index", "Home");
        
        return View(loginUserDto);
    }

    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<IActionResult> Register(RegisterUserDto registerUserDto)
    {
        if (!ModelState.IsValid)
            return View(registerUserDto);
        
        var user = await _userService.RegisterAsync(registerUserDto);
        if (user is not null)
        {
            await _userService.Add(user);
            return RedirectToAction("Login", "Home");
        }
        
        return View(registerUserDto);
    }
}