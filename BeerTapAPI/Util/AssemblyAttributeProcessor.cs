using System.Reflection;

/*

    Abstract class that implements boilerplate functionality for iterating
    over all type definitions in a given set of assemblies. 

    ProcessItem<T>(TAttribute attribute) will be called for every Type T
    tagged with the custom attribute [TAttribute].

*/

public abstract class AssemblyAttrbiuteProcessor<TAttribute> where TAttribute : Attribute
{
    protected IServiceCollection Collection;

    protected AssemblyAttrbiuteProcessor(IServiceCollection collection) { Collection = collection; }

    protected void Process(Assembly assembly, params Assembly[] assemblies)
    {
        Process(assembly);
        foreach (var a in assemblies) Process(a);
    }

    private void Process(Assembly assembly)
    {
        IList<(Type, TAttribute)> services = FindItemsInAssembly(assembly);
        foreach (var (clazz, attribute) in services) ProcessItem(clazz, attribute);
    }
    private IList<(Type, TAttribute)> FindItemsInAssembly(Assembly assembly)
    {
        var result = new List<(Type, TAttribute)>();
        foreach (var clazz in assembly.GetTypes())
        {
            var attributes = clazz.GetCustomAttributes<TAttribute>(true);
            foreach (var attribute in attributes) result.Add((clazz, attribute));
        }
        return result;
    }

    private void ProcessItem(Type clazz, TAttribute attribute)
    {
        // Find and call the generic abstract method ProcessItem<T>(TAttribute attribute)
        var ThisType = GetType();
        var AttributeType = typeof(TAttribute);

        // Reflection magic to get a reference to our taget function
        MethodInfo? GenericProcessItem = ThisType.GetRuntimeMethods()
            .Where(m => m.Name == "ProcessItem") // WARN: hardcoded function name!
            .Select(m => new
            {
                Method = m,
                Params = m.GetParameters(),
                Args = m.GetGenericArguments()
            })
            .Where(x => x.Params.Length == 1 && x.Params[0].ParameterType == typeof(TAttribute)
                        && x.Args.Length == 1)
            .Select(x => x.Method)
            .FirstOrDefault();

        if (GenericProcessItem is null) throw new NullReferenceException($"unable to reflect on '{ThisType.Name}.ProcessItem<{clazz.Name}>({AttributeType.Name})'");

        // Call ProcessItem<clazz>(attribute);
        GenericProcessItem
            .MakeGenericMethod(clazz)
            .Invoke(this, new object?[] { attribute });
    }


    protected abstract void ProcessItem<TClass>(TAttribute attribute) where TClass : class;

}