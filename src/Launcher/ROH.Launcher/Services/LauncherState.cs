using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using ROH.Utils.ApiConfiguration;
using ROH.Utils.Helpers;
using ROH.StandardModels.Account;
using ROH.StandardModels.Response;

namespace ROH.Launcher.Services
{
    // Launcher state service for UI binding and gateway integration
    public class LauncherState
    {
        public event Action? OnChange;

        readonly SettingsService _settings;
        readonly AlertService _alert;

        public LauncherState(SettingsService settings, AlertService alert)
        {
            _settings = settings;
            _alert = alert;
            _ = LoadTokenAsync();
        }

        public bool IsUpdating { get; private set; }
        public double UpdateProgress { get; private set; } // 0-100

        // Login info
        public bool IsLoggedIn { get; private set; }
        public string? Username { get; private set; }
        public string? Token { get; private set; }

        readonly Gateway _gateway = new();

        CancellationTokenSource? _updateCts;

        async Task LoadTokenAsync()
        {
            try
            {
                string? token = await Microsoft.Maui.Storage.SecureStorage.Default.GetAsync("ROH_LAUNCHER_TOKEN");
                if (!string.IsNullOrWhiteSpace(token))
                {
                    Token = token;
                    IsLoggedIn = true; // assume logged in if token exists; optionally validate
                    NotifyChanged();
                }
            }
            catch
            {
                // ignore secure storage failures
            }
        }

        async Task SaveTokenAsync(string token)
        {
            try
            {
                await Microsoft.Maui.Storage.SecureStorage.Default.SetAsync("ROH_LAUNCHER_TOKEN", token);
            }
            catch
            {
            }
        }

        async Task ClearTokenAsync()
        {
            try
            {
                await Microsoft.Maui.Storage.SecureStorage.Default.SetAsync("ROH_LAUNCHER_TOKEN", string.Empty);
            }
            catch
            {
            }
        }

        public async Task<bool> LoginAsync(LoginModel model, CancellationToken cancellationToken = default)
        {
            DefaultResponse? response = await _gateway.PostAsync(Gateway.Services.Login, model, string.Empty, cancellationToken)
                .ConfigureAwait(true);

            if ((response != null) && response.HttpStatus.IsSuccessStatusCode())
            {
                try
                {
                    UserModel user = response.ResponseToModel<UserModel>();
                    Username = user.UserName ?? user.Email;
                    Token = user.Token;
                    IsLoggedIn = true;
                    await SaveTokenAsync(Token ?? string.Empty).ConfigureAwait(true);
                    await _alert.ShowInfo("Login", "Login successful").ConfigureAwait(true);
                    NotifyChanged();
                    return true;
                }
                catch (Exception ex)
                {
                    await _alert.ShowError("Login", "Failed to process login response: " + ex.Message).ConfigureAwait(true);
                    return false;
                }
            }

            await _alert.ShowError("Login", "Invalid credentials or server error").ConfigureAwait(true);
            return false;
        }

        public async Task Logout()
        {
            Username = null;
            Token = null;
            IsLoggedIn = false;
            await ClearTokenAsync().ConfigureAwait(true);
            NotifyChanged();
        }

        public void StartUpdate(Func<IProgress<double>, CancellationToken, Task<string?>>? updater = null)
        {
            if (IsUpdating)
                return;

            IsUpdating = true;
            UpdateProgress = 0;
            _updateCts = new CancellationTokenSource();

            if (updater is null)
            {
                // fallback simulated progress
                _ = Task.Run(async () =>
                {
                    var progress = new Progress<double>(p => { UpdateProgress = p; NotifyChanged(); });
                    double pVal = 0;
                    while (!_updateCts.IsCancellationRequested && pVal < 100)
                    {
                        await Task.Delay(200).ConfigureAwait(true);
                        pVal += 2 + (new Random().NextDouble() * 4);
                        ((IProgress<double>)progress).Report(Math.Min(100, pVal));
                    }

                    IsUpdating = false;
                    NotifyChanged();
                }, _updateCts.Token);
            }
            else
            {
                var progress = new Progress<double>(p => { UpdateProgress = p; NotifyChanged(); });
                _ = Task.Run(async () =>
                {
                    string? installedPath = null;
                    try
                    {
                        installedPath = await updater(progress, _updateCts.Token).ConfigureAwait(true);
                    }
                    catch (OperationCanceledException)
                    {
                        // cancelled by user
                    }
                    catch (Exception ex)
                    {
                        // show alert to user
                        try
                        {
                            await _alert.ShowError("Update", ex.Message).ConfigureAwait(true);
                        }
                        catch
                        {
                        }
                    }
                    finally
                    {
                        IsUpdating = false;
                        NotifyChanged();

                        if (!string.IsNullOrWhiteSpace(installedPath))
                        {
                            try
                            {
                                await _alert.ShowInfo("Update", "Update completed successfully").ConfigureAwait(true);
                            }
                            catch
                            {
                            }

                            // attempt to launch game automatically
                            try
                            {
                                await LaunchGameAsync(installedPath!).ConfigureAwait(true);
                            }
                            catch (Exception ex)
                            {
                                try
                                {
                                    await _alert.ShowError("Launch", "Failed to launch after update: " + ex.Message).ConfigureAwait(true);
                                }
                                catch
                                {
                                }
                            }
                        }
                    }
                }, _updateCts.Token);
            }
        }

        public void StopUpdate()
        {
            if (!_updateCts?.IsCancellationRequested ?? false)
                _updateCts?.Cancel();
            IsUpdating = false;
            NotifyChanged();
        }

        public async Task LaunchGameAsync(string installPath)
        {
            try
            {
                // Platform specific: only implement Windows + macOS simple starts
#if WINDOWS
                string exe = Path.Combine(installPath, "ROH.exe");
                if (File.Exists(exe))
                {
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(exe) { UseShellExecute = true });
                }
#elif MACCATALYST || MACOS
                string app = Path.Combine(installPath, "ROH.app");
                if (Directory.Exists(app))
                {
                    System.Diagnostics.Process.Start("open", $"\"{app}\"");
                }
#endif
            }
            catch (Exception ex)
            {
                await _alert.ShowError("Launch", "Failed to launch game: " + ex.Message).ConfigureAwait(true);
            }
        }

        void NotifyChanged() => OnChange?.Invoke();
    }
}
