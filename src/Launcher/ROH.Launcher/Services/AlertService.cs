namespace ROH.Launcher.Services;

public enum AlertKind
{
    Success,
    Error,
}

public sealed class LauncherAlert
{
    public string Title { get; init; } = string.Empty;

    public string Message { get; init; } = string.Empty;

    public AlertKind Kind { get; init; }
}

public class AlertService
{
    public event Action<LauncherAlert>? OnAlert;

    public Task ShowSuccess(string title, string message) => Show(title, message, AlertKind.Success);

    public Task ShowInfo(string title, string message) => ShowSuccess(title, message);

    public Task ShowError(string title, string message) => Show(title, message, AlertKind.Error);

    private Task Show(string title, string message, AlertKind kind)
    {
        Console.WriteLine($"{kind}: {title} - {message}");
        OnAlert?.Invoke(
            new LauncherAlert
            {
                Title = title,
                Message = message,
                Kind = kind,
            });

        return Task.CompletedTask;
    }
}
