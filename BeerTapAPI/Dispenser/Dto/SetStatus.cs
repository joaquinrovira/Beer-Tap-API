namespace BeerTapAPI.Dtos;

public enum DispenserStatus
{
    open,
    close
}

public record SetDispenserStatusRequest(
    DispenserStatus status,
    DateTime? updated_at
)
{ }

public record SetDispenserStatusResponse(
    DispenserStatus status,
    DateTime updated_at
)
{ }