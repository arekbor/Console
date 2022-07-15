using Console.Hubs;
using Console.Contracts;
using Console.Infrastructure;
using Console.Models;
using Console.Persistence;
using Console.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);

ConfigurationHelperService.Init(builder.Configuration);

var cookieScheme = ConfigurationHelperService.config.GetSection("CookieAuthenticationName").Value;

builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();

builder.Services.AddAuthentication(cookieScheme)
    .AddCookie(cookieScheme, option =>
    {
        option.Cookie.Name = cookieScheme;
        option.LoginPath = "/home/login";
        option.AccessDeniedPath = "/home/forbidden";
        option.ReturnUrlParameter = "return";
    });

builder.Services.AddDbContext<AppDbContext>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserContextService, UserContextService>();
builder.Services.AddScoped<ICommandService, CommandService>();
builder.Services.AddScoped<IHubService, HubService>();

//builder.Services.Configure<KestrelServerOptions>(ConfigurationHelperService.config.GetSection("Kestrel"));

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default", 
    pattern: "{controller=Home}/{action=Index}"
    );

app.MapHub<ConsoleHub>("/console");

app.Run();