using System;
using System.IO;
using System.Text.Json;

namespace ROH.Launcher.Services
{
    public class SettingsService
    {
        readonly ConfirmService _confirmService;
        readonly IFolderPicker _folderPicker;

        public SettingsService(ConfirmService confirmService, IFolderPicker folderPicker)
        {
            _confirmService = confirmService;
            _folderPicker = folderPicker;
        }

        // Simple settings holder persisted to disk.
        const string SETTINGS_FILE = "launcher-settings.json";

        public string InstallDirectory { get; set; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ROH", "game");

        public string GameExecutableName { get; set; } = "ReignOfHumanae.exe";

        public string LastInstalledVersion { get; set; } = string.Empty;

        public DateTime? LastUpdatedAtUtc { get; set; }

        public async Task LoadAsync()
        {
            try
            {
                string folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ROH");
                string path = Path.Combine(folder, SETTINGS_FILE);
                if (!File.Exists(path))
                    return;

                string json = await File.ReadAllTextAsync(path).ConfigureAwait(true);
                var doc = JsonDocument.Parse(json);
                if (doc.RootElement.TryGetProperty("InstallDirectory", out var node))
                {
                    string v = node.GetString() ?? string.Empty;
                    if (!string.IsNullOrWhiteSpace(v))
                        InstallDirectory = v;
                }

                if (doc.RootElement.TryGetProperty("GameExecutableName", out var executableNode))
                {
                    string executable = executableNode.GetString() ?? string.Empty;
                    if (!string.IsNullOrWhiteSpace(executable))
                        GameExecutableName = executable;
                }

                if (doc.RootElement.TryGetProperty("LastInstalledVersion", out var versionNode))
                    LastInstalledVersion = versionNode.GetString() ?? string.Empty;

                if (doc.RootElement.TryGetProperty("LastUpdatedAtUtc", out var updatedNode) &&
                    updatedNode.ValueKind != JsonValueKind.Null &&
                    updatedNode.TryGetDateTime(out DateTime updatedAt))
                {
                    LastUpdatedAtUtc = updatedAt;
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
                // validate install directory exists - request confirmation to create if needed
                if (string.IsNullOrWhiteSpace(InstallDirectory))
                    return false;

                if (!Directory.Exists(InstallDirectory))
                {
                    bool ok = await _confirmService.RequestConfirm(
                            "Criar pasta?",
                            $"A pasta de instalacao '{InstallDirectory}' nao existe. Deseja cria-la?")
                        .ConfigureAwait(true);

                    if (!ok)
                        return false;

                    try
                    {
                        Directory.CreateDirectory(InstallDirectory);
                    }
                    catch
                    {
                        return false;
                    }
                }

                await WriteSettingsAsync().ConfigureAwait(true);
                // return whether install directory exists after save
                return !string.IsNullOrWhiteSpace(InstallDirectory) && Directory.Exists(InstallDirectory);
            }
            catch
            {
                // ignore
                return false;
            }
        }

        public async Task SaveInstallMetadataAsync(string versionLabel)
        {
            LastInstalledVersion = versionLabel;
            LastUpdatedAtUtc = DateTime.UtcNow;
            await WriteSettingsAsync().ConfigureAwait(true);
        }

        public async Task<string?> PickInstallDirectoryAsync()
        {
            try
            {
                var picked = await _folderPicker.PickFolderAsync().ConfigureAwait(true);
                if (!string.IsNullOrWhiteSpace(picked))
                {
                    InstallDirectory = picked;
                    return picked;
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

        async Task WriteSettingsAsync()
        {
            string folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ROH");
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            string path = Path.Combine(folder, SETTINGS_FILE);
            var doc = new
            {
                InstallDirectory,
                GameExecutableName,
                LastInstalledVersion,
                LastUpdatedAtUtc,
            };
            string json = JsonSerializer.Serialize(doc, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(path, json).ConfigureAwait(true);
        }
    }
}
