using System;
using System.Threading.Tasks;
using System.Timers;

namespace ROH.Launcher.Services
{
    // Simple in-memory launcher state service for UI binding
    public class LauncherState
    {
        public event Action? OnChange;

        public bool IsUpdating { get; private set; }
        public double UpdateProgress { get; private set; } // 0-100

        // Simple properties for login/settings
        public bool IsLoggedIn { get; private set; }
        public string? Username { get; private set; }

        private System.Timers.Timer? _timer;

        public void StartUpdate()
        {
            if (IsUpdating)
                return;

            IsUpdating = true;
            UpdateProgress = 0;

            _timer = new System.Timers.Timer(200);
            _timer.Elapsed += Timer_Elapsed;
            _timer.Start();

            NotifyChanged();
        }

        private void Timer_Elapsed(object? sender, ElapsedEventArgs e)
        {
            UpdateProgress += 2 + (new Random().NextDouble() * 4);
            if (UpdateProgress >= 100)
            {
                UpdateProgress = 100;
                StopUpdate();
            }
            NotifyChanged();
        }

        public void StopUpdate()
        {
            _timer?.Stop();
            _timer?.Dispose();
            _timer = null;
            IsUpdating = false;
            NotifyChanged();
        }

        public void SimulateLogin(string username)
        {
            Username = username;
            IsLoggedIn = true;
            NotifyChanged();
        }

        public void Logout()
        {
            Username = null;
            IsLoggedIn = false;
            NotifyChanged();
        }

        private void NotifyChanged() => OnChange?.Invoke();
    }
}
