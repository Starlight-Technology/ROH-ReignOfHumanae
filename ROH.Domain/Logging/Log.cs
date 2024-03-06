namespace ROH.Domain.Logging;
public record Log(long Id, Severity Severity, string Message)
{

    public DateTime Date { get; } = DateTime.Now;
}
