namespace BeerTapAPI.Errors;

public class Error : Exception
{
    public readonly string ErrorStackTrace = Environment.StackTrace;
    public Error(string? msg = null, Exception? inner = null) : base(msg, inner) { }
    public static Error From(Exception e) => new Error(e.Message, e);
}

public class Conflict : Error
{
    public Conflict(string? msg) : base(msg) { }
}