using BeerTapAPI.Entities;
using BeerTapAPI.Errors;
using CSharpFunctionalExtensions;

public interface IDispenserRepository
{
    Result<Dispenser, Error> Get(Guid id);
    UnitResult<Error> Register(Dispenser d);
    UnitResult<Error> PublishEvent(Guid id, IDispenserEvent e);
    Result<Maybe<IDispenserEvent>, Error> LastEvent(Guid id);
    Result<IEnumerable<IDispenserEvent>, Error> UsageReportByDate(Guid id);
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

    public Result<Dispenser, Error> Get(Guid id)
    {
        if (!Dispensers.ContainsKey(id)) return new Error($"dispenser with id '{id}' not found");
        return Dispensers[id];
    }



    public Result<Maybe<IDispenserEvent>, Error> LastEvent(Guid id)
    {
        return Get(id).Map(d =>
        {
            var set = Events[d.Id];
            if (set.Count == 0) return Maybe.None;
            return Maybe.From(Events[id].Last());
        });
    }

    public UnitResult<Error> PublishEvent(Guid id, IDispenserEvent e)
    {
        return Get(id).Bind(d =>
        {
            Events[d.Id].Add(e);
            return UnitResult.Success<Error>();
        });
    }

    public Result<IEnumerable<IDispenserEvent>, Error> UsageReportByDate(Guid id)
    {
        return Get(id).Map(d => Events[d.Id].AsEnumerable());
    }
}