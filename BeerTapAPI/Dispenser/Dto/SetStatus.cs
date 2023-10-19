namespace BeerTapAPI.Dtos;

public enum DispenserStatus
{
    open,
    close
}

public record SetDispenserStatusRequest(
    DispenserStatus Status,
    DateTime? UpdatedAt
)
{ }

public record SetDispenserStatusResponse(
    DispenserStatus Status,
    DateTime UpdatedAt
)
{ }