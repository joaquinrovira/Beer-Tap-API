namespace BeerTapAPI.Events;

public record class OpenTapEvent(Guid DispenserId, DateTime At) : IDispenserEvent
{
    DateTime IDispenserEvent.At() => At;
}