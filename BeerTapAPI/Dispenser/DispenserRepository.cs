using BeerTapAPI.Entities;
using BeerTapAPI.Util;
using CSharpFunctionalExtensions;

public interface IDispenserRepository
{
    UnitResult<Error> Register(Dispenser d);
}

[Service]
public record class MemoryDispenserRepository() : IDispenserRepository
{
    Dictionary<Guid, Dispenser> Dispensers = new();
    Dictionary<Guid, SortedSet<IDispenserEvent>> Events = new();

    public UnitResult<Error> Register(Dispenser d)
    {
        var dispenser = WithId.From(d);
        Dispensers.Add(dispenser.Id, dispenser.Item);
        Events.Add(dispenser.Id, new(DispenserEventComparer.Default));
        return UnitResult.Success<Error>();
    }
}