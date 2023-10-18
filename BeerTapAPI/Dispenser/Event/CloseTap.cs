namespace BeerTapAPI.Events;

public record class CloseTapEvent(Guid DispenserId, DateTime At) : IDispenserEvent
{
    DateTime IDispenserEvent.At() => At;
}