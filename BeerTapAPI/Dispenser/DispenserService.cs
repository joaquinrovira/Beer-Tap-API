using BeerTapAPI.Dtos;
using BeerTapAPI.Entities;
using BeerTapAPI.Errors;
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
}