namespace Console.Services;

public class ConfigurationHelperService
{
    public static IConfiguration config;

    public static void Init(IConfiguration configuration)
    {
        config = configuration;
    }
}