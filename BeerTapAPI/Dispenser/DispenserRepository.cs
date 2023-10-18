using BeerTapAPI.Entities;
using BeerTapAPI.Errors;
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
        Dispensers.Add(d.Id, d);
        Events.Add(d.Id, new(DispenserEventComparer.Default));
        return Result.Success<Dispenser, Error>(d);
    }
}