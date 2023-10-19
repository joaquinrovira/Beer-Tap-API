using Microsoft.Extensions.DependencyInjection;
using Moq;

public static class MoqExtensions
{
    public static void AddMock<TInterface>(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Scoped, MockBehavior behavior = MockBehavior.Loose)
        where TInterface : class
        => AddMock<TInterface, TInterface>(services, lifetime, behavior);

    public static void AddMock<TInterface, TImplementation>(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Scoped, MockBehavior behavior = MockBehavior.Loose)
        where TInterface : class
        where TImplementation : class, TInterface
    {
        services.AddScoped(s => new Mock<TImplementation>(behavior, ConstructorParams<TImplementation>(s)));
        services.AddScoped(s => s.GetRequiredService<Mock<TImplementation>>().Object);
        if (typeof(TInterface) != typeof(TImplementation))
            services.AddScoped(s => s.GetRequiredService<Mock<TImplementation>>().Object as TInterface);
    }

    private static object[] ConstructorParams<T>(IServiceProvider s)
    {
        var contructors = typeof(T).GetConstructors();
        if (contructors.Length > 1) throw new Exception("too many constructors");

        var parameters = new object[] { };
        if (contructors.Length == 0) return parameters;

        var types = contructors.Single().GetParameters().Select(p => p.ParameterType);
        foreach (var type in types) parameters.Append(s.GetRequiredService(type));
        return parameters;
    }
}