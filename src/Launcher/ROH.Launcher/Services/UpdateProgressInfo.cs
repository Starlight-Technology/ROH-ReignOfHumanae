namespace ROH.Launcher.Services;

public sealed class UpdateProgressInfo
{
    public double Percentage { get; init; }

    public int CompletedFiles { get; init; }

    public int TotalFiles { get; init; }

    public string CurrentFileName { get; init; } = string.Empty;

    public string Message { get; init; } = string.Empty;
}
