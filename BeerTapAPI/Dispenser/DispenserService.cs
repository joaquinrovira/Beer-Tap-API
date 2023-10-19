using BeerTapAPI.Dtos;
using BeerTapAPI.Entities;
using BeerTapAPI.Errors;
using BeerTapAPI.Events;
using CSharpFunctionalExtensions;

[Service]
public record class DispenserService(IDispenserRepository DispenserRepository)
{
    public const decimal PRICE = 12.25M;

    public Result<Dispenser, Error> Register(RegisterDispenserRequest request)
    {
        var dispenser = new Dispenser(Guid.NewGuid(), request.FlowVolume);
        return DispenserRepository
            .Register(dispenser)
            .Map(() => dispenser);
    }

    public Result<SetDispenserStatusResponse, Error> SetStatus(Guid id, SetDispenserStatusRequest data)
    {
        return ValidateStatusRequest(id, data).Bind(() =>
        {
            var time = data.UpdatedAt.GetValueOrDefault(DateTime.Now);

            var result = data.Status switch
            {
                DispenserStatus.open => OpenTap(id, time),
                DispenserStatus.close => CloseTap(id, time),
                _ => new Error($"not implemented '{data.Status}'")
            };

            return result.Map(() => new SetDispenserStatusResponse(data.Status, time));
        });
    }

    private UnitResult<Error> ValidateStatusRequest(Guid id, SetDispenserStatusRequest data)
    {
        return DispenserRepository.LastEvent(id).Bind(evt =>
        {
            if (evt.HasNoValue && data.Status == DispenserStatus.close) return new Conflict();
            if (evt.HasValue && evt.Value is CloseTapEvent && data.Status == DispenserStatus.close) return new Conflict();
            if (evt.HasValue && evt.Value is OpenTapEvent && data.Status == DispenserStatus.open) return new Conflict();
            return UnitResult.Success<Error>();
        });
    }

    public UnitResult<Error> OpenTap(Guid id, DateTime at)
    {
        var evt = new OpenTapEvent(id, at);
        return DispenserRepository.PublishEvent(id, evt);
    }
    public UnitResult<Error> CloseTap(Guid id, DateTime at)
    {
        var evt = new CloseTapEvent(id, at);
        return DispenserRepository.PublishEvent(id, evt);
    }

    public Result<DispenserUsageReportResponse, Error> UsageReport(Guid id)
    {
        return DispenserRepository
            .Get(id)
            .Bind(d =>
                DispenserRepository
                .DispenserEvents(id)
                .Map(e => new { Dispenser = d, Events = e })
            )
            .Map(v => Usages(v.Dispenser, v.Events));
    }

    private DispenserUsageReportResponse Usages(Dispenser d, IEnumerable<IDispenserEvent> events)
    {
        var report = new List<DispenserUsageReportResponseItem>();
        var total = 0M;
        var item = Maybe<UsageReportItem>.None;
        foreach (var evt in events)
        {
            if (item.HasNoValue && evt is OpenTapEvent) item = new UsageReportItem(d, evt.At());
            if (item.HasValue && evt is CloseTapEvent)
            {
                item.Value.SetClosingTime(evt.At());
                report.Add(item.Value.Commit());
                total += item.Value.TotalSpent;
                item = Maybe.None;
            }
        }
        if (item.HasValue)
        {
            report.Add(item.Value.Commit());
            total += item.Value.TotalSpent;
        }
        return new(total, report);
    }

}

public class UsageReportItem
{
    Dispenser Dispenser;
    public DateTime OpenedAt { get; }
    public DateTime ClosedAt => MaybeClosedAt.GetValueOrDefault(ImplicitClosedAt);
    public decimal FlowVolume => (decimal)(ClosedAt - OpenedAt).TotalSeconds * (decimal)Dispenser.FlowVolume;
    public decimal TotalSpent => FlowVolume * DispenserService.PRICE;
    Maybe<DateTime> MaybeClosedAt = Maybe.None;
    DateTime ImplicitClosedAt = DateTime.Now;

    public UsageReportItem(Dispenser dispenser, DateTime openedAt)
    {
        Dispenser = dispenser;
        OpenedAt = openedAt;
    }
    public void SetClosingTime(DateTime t)
    {
        MaybeClosedAt = t;
    }

    public DispenserUsageReportResponseItem Commit() => new(OpenedAt, MaybeClosedAt.AsNullable(), FlowVolume, TotalSpent);
}
