using Microsoft.Extensions.DependencyInjection;

public static class ServiceRegisty
{
    public static void Register(IServiceCollection Serivces)
    {
        Serivces.AddScoped<DispenserService>();
        Serivces.AddMock<IDispenserRepository>();
    }
}