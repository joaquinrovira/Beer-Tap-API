namespace BeerTapAPI.Dtos;

public record RegisterDispenserRequest(
    float FlowVolume
)
{ }

public record RegisterDispenserResponse(
    Guid id,
    float FlowVolume
)
{ }