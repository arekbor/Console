using Console.Models;
using Microsoft.EntityFrameworkCore;

namespace Console.Persistence;

public class AppDbContext:DbContext
{
    private readonly IConfiguration _configuration;
    public AppDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public DbSet<User> DbUserContext { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
        base.OnConfiguring(optionsBuilder);
    }
}
