public interface IDispenserEvent
{
    DateTime At();
}

public class DispenserEventType
{
    private DispenserEventType(string value) { Value = value; }

    public string Value { get; private set; }
    public override string ToString() => Value;

    public static DispenserEventType OpenTap { get { return new DispenserEventType("BeerTap.V1.Dispenser.OpenTap.V1"); } }
    public static DispenserEventType CloseTap { get { return new DispenserEventType("BeerTap.V1.Dispenser.CloseTap.V1"); } }

}


public class DispenserEventComparer : IComparer<IDispenserEvent>
{
    public static DispenserEventComparer Default { get; } = new DispenserEventComparer();
    public int Compare(IDispenserEvent? a, IDispenserEvent? b)
    {
        if (a is null) return 1; // y < x
        if (b is null) return -1;// x < y
        return a.At().CompareTo(b.At());
    }
}