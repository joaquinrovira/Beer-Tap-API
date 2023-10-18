using System.ComponentModel.DataAnnotations;

namespace BeerTapAPI.Dtos;

public record RegisterDispenserRawRequest(
    [Required]
    float? FlowVolume
)
{
    public RegisterDispenserRequest Checked() => new RegisterDispenserRequest(FlowVolume!.Value);
}
public record RegisterDispenserRequest(
    float FlowVolume
)
{ }

public record RegisterDispenserResponse(
    Guid id,
    float FlowVolume
)
{ }