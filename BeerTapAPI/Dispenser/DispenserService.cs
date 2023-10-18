
using BeerTapAPI.Dtos;
using BeerTapAPI.Entities;

[Service]
public record class DispenserService(IDispenserRepository DispenserRepository)
{

    public void Register(RegisterDispenserRequest request)
    {
        var dispenser = new Dispenser(request.FlowVolume);
        DispenserRepository.Register(dispenser);
    }
}