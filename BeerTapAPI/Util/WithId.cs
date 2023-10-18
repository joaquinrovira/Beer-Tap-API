namespace BeerTapAPI.Util;

public class WithId<T>
{
    public Guid Id { get; } = Guid.NewGuid();
    public T Item { get; }
    public WithId(T item)
    {
        Item = item;
    }
}

public class WithId
{
    public static WithId<T> From<T>(T item) => new WithId<T>(item);
}