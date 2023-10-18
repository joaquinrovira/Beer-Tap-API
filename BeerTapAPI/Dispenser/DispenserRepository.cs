using BeerTapAPI.Entities;
using BeerTapAPI.Util;

public interface IDispenserRepository
{
    void Register(Dispenser d);
}

[Service]
public record class MemoryDispenserRepository() : IDispenserRepository
{
    Dictionary<Guid, Dispenser> Dispensers = new();
    Dictionary<Guid, SortedSet<IDispenserEvent>> Events = new();

    public void Register(Dispenser d)
    {
        var dispenser = WithId.From(d);
        Dispensers.Add(dispenser.Id, dispenser.Item);
        Events.Add(dispenser.Id, new(DispenserEventComparer.Default));
    }
}