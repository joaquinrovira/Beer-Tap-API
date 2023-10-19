using Microsoft.Extensions.DependencyInjection;
using Moq;
internal class NUnitDI
{
    private IServiceProvider Root;
    private IServiceProvider? ScopedSerivces;
    protected IServiceProvider Services => ScopedSerivces ?? Root;
    protected T Get<T>() where T : class => Services.GetRequiredService<T>();
    protected Mock<T> GetMock<T>() where T : class => Services.GetRequiredService<Mock<T>>();

    [OneTimeSetUp]
    public void InitializeDIContainer()
    {
        var collection = new ServiceCollection();
        ServiceRegisty.Register(collection);
        Root = collection.BuildServiceProvider();
    }

    [SetUp]
    public void NewScope()
    {
        ScopedSerivces = Root.CreateScope().ServiceProvider;
    }
    [TearDown]
    public void DiscardScope()
    {
        ScopedSerivces = null;
    }
}