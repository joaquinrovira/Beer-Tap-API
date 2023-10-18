using BeerTapAPI.Dtos;
using BeerTapAPI.Entities;
using BeerTapAPI.Errors;
using BeerTapAPI.Events;
using CSharpFunctionalExtensions;

[Service]
public record class DispenserService(IDispenserRepository DispenserRepository)
{

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

    public UnitResult<Error> ValidateStatusRequest(Guid id, SetDispenserStatusRequest data)
    {
        return DispenserRepository.LastEvent(id).Ensure(evt =>
        {
            if (evt.HasNoValue) return true;
            if (evt.Value is CloseTapEvent && data.Status == DispenserStatus.close) return false;
            if (evt.Value is OpenTapEvent && data.Status == DispenserStatus.open) return false;
            return true;
        }, new Conflict($"Dispenser is already opened/closed"));
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

}