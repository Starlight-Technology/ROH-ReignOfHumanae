using System;
using System.Threading.Tasks;

namespace ROH.Launcher.Services
{
    // Simple alert service - currently writes to Console but can be replaced with platform UI (Maui Alerts) or JS toast
    public class AlertService
    {
        public event Action<string, string, bool>? OnAlert;
        public Task ShowInfo(string title, string message)
        {
            Console.WriteLine($"INFO: {title} - {message}");
            OnAlert?.Invoke(title, message, false);
            return Task.CompletedTask;
        }

        public Task ShowError(string title, string message)
        {
            Console.WriteLine($"ERROR: {title} - {message}");
            OnAlert?.Invoke(title, message, true);
            return Task.CompletedTask;
        }
    }
}
