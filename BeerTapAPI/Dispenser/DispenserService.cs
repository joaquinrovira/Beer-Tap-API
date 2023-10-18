
using BeerTapAPI.Dtos;
using BeerTapAPI.Entities;
using CSharpFunctionalExtensions;

[Service]
public record class DispenserService(IDispenserRepository DispenserRepository)
{

    public UnitResult<Error> Register(RegisterDispenserRequest request)
    {
        var dispenser = new Dispenser(request.FlowVolume);
        return DispenserRepository.Register(dispenser);
    }
}