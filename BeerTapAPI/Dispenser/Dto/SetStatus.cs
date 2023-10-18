namespace BeerTapAPI.Dtos;

// NOTE: declared here for now, will probably move later
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