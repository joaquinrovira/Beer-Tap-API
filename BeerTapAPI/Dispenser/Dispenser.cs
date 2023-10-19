namespace BeerTapAPI.Entities;

public record class Dispenser(
    Guid Id,
    float FlowVolume
)
{ }