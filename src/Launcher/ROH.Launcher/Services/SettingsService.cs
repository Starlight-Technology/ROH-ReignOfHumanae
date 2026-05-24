using System;
using System.IO;

namespace ROH.Launcher.Services
{
    public class SettingsService
    {
        readonly ConfirmService _confirmService;

        public SettingsService(ConfirmService confirmService)
        {
            _confirmService = confirmService;
        }
        // Simple settings holder persisted to disk.
        const string SETTINGS_FILE = "launcher-settings.json";

        public string InstallDirectory { get; set; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ROH", "game");

        public async Task LoadAsync()
        {
            try
            {
                string folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ROH");
                string path = Path.Combine(folder, SETTINGS_FILE);
                if (!File.Exists(path))
                    return;

                string json = await File.ReadAllTextAsync(path).ConfigureAwait(true);
                var doc = System.Text.Json.JsonDocument.Parse(json);
                if (doc.RootElement.TryGetProperty("InstallDirectory", out var node))
                {
                    string v = node.GetString() ?? string.Empty;
                    if (!string.IsNullOrWhiteSpace(v))
                        InstallDirectory = v;
                }
            }
            catch
            {
                // ignore
            }
        }

        public async Task<bool> SaveAsync()
        {
            try
            {
                string folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ROH");
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                // validate install directory exists - request confirmation to create if needed
                try
                {
                    if (!string.IsNullOrWhiteSpace(InstallDirectory) && !Directory.Exists(InstallDirectory))
                    {
                        // ask for confirmation via injected ConfirmService
                        bool ok = await _confirmService.RequestConfirm("Create folder?", $"Install directory '{InstallDirectory}' does not exist. Create it?").ConfigureAwait(true);

                        if (ok)
                        {
                            try
                            {
                                Directory.CreateDirectory(InstallDirectory);
                            }
                            catch
                            {
                                // creation failed
                            }
                        }
                    }
                }
                catch
                {
                    // ignore installation dir creation failures
                }

                string path = Path.Combine(folder, SETTINGS_FILE);
                var doc = new { InstallDirectory };
                string json = System.Text.Json.JsonSerializer.Serialize(doc, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
                await File.WriteAllTextAsync(path, json).ConfigureAwait(true);
                // return whether install directory exists after save
                return !string.IsNullOrWhiteSpace(InstallDirectory) && Directory.Exists(InstallDirectory);
            }
            catch
            {
                // ignore
                return false;
            }
        }

        public async Task<string?> PickInstallDirectoryAsync()
        {
            try
            {
                // resolve platform-specific folder picker via DI
                var picker = MauiProgram.CreateMauiApp().Services.GetService(typeof(IFolderPicker)) as IFolderPicker;
                if (picker != null)
                {
                    var picked = await picker.PickFolderAsync().ConfigureAwait(true);
                    if (!string.IsNullOrWhiteSpace(picked))
                    {
                        InstallDirectory = picked;
                        return picked;
                    }
                }

                // fallback
                var result = await Microsoft.Maui.Storage.FilePicker.Default.PickAsync().ConfigureAwait(true);
                if (result == null)
                    return null;

                if (!string.IsNullOrWhiteSpace(result.FullPath))
                {
                    var dir = Path.GetDirectoryName(result.FullPath);
                    InstallDirectory = dir ?? InstallDirectory;
                    return dir;
                }

                return null;
            }
            catch
            {
                return null;
            }
        }
    }
}
