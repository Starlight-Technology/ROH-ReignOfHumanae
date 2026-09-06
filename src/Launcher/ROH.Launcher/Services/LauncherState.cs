using Newtonsoft.Json;

using ROH.StandardModels.Account;
using ROH.StandardModels.Response;

using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace ROH.Launcher.Services;

public class LauncherState
{
    private readonly AlertService _alert;
    private readonly SettingsService _settings;
    private CancellationTokenSource? _updateCts;
    private bool _settingsLoaded;

    public LauncherState(SettingsService settings, AlertService alert)
    {
        _settings = settings;
        _alert = alert;
        _ = InitializeAsync();
    }

    public event Action? OnChange;

    public int AvailableFiles { get; private set; }

    public int CompletedFiles { get; private set; }

    public string CurrentFileName { get; private set; } = string.Empty;

    public string InstallDirectory => _settings.InstallDirectory;

    public bool InstallDirectoryExists =>
        !string.IsNullOrWhiteSpace(_settings.InstallDirectory) &&
        Directory.Exists(_settings.InstallDirectory);

    public string InstalledVersionLabel => string.IsNullOrWhiteSpace(_settings.LastInstalledVersion)
        ? "Nao instalado"
        : _settings.LastInstalledVersion;

    public bool IsInstalled => FindGameExecutable() is not null;

    public bool IsLoggedIn { get; private set; }

    public bool IsReadyToPlay => !IsUpdating && IsInstalled;

    public bool IsUpdating { get; private set; }

    public DateTime? LastVersionCheckUtc { get; private set; }

    public string LastUpdatedLabel => _settings.LastUpdatedAtUtc.HasValue
        ? _settings.LastUpdatedAtUtc.Value.ToLocalTime().ToString("dd/MM/yyyy HH:mm")
        : "Nunca";

    public string LatestVersionLabel { get; private set; } = "Nao verificada";

    public string StatusMessage { get; private set; } = "Inicializando launcher";

    public string? Token { get; private set; }

    public int TotalFiles { get; private set; }

    public CancellationToken UpdateCancellationToken => _updateCts?.Token ?? CancellationToken.None;

    public double UpdateProgress { get; private set; }

    public string? Username { get; private set; }

    public bool BeginUpdate(string message)
    {
        if (IsUpdating)
        {
            _ = _alert.ShowError("Atualizacao", "Ja existe uma atualizacao em andamento.");
            return false;
        }

        _updateCts?.Dispose();
        _updateCts = new CancellationTokenSource();
        IsUpdating = true;
        UpdateProgress = 0;
        CompletedFiles = 0;
        TotalFiles = 0;
        CurrentFileName = string.Empty;
        StatusMessage = message;
        NotifyChanged();
        return true;
    }

    public async Task CancelUpdateAsync()
    {
        IsUpdating = false;
        UpdateProgress = 0;
        CurrentFileName = string.Empty;
        StatusMessage = IsInstalled ? "Jogo pronto" : "Atualizacao cancelada";
        NotifyChanged();
        await _alert.ShowInfo("Atualizacao", "Operacao cancelada.").ConfigureAwait(true);
    }

    public async Task CompleteUpdateAsync(string versionLabel)
    {
        IsUpdating = false;
        UpdateProgress = 100;
        CompletedFiles = TotalFiles;
        CurrentFileName = string.Empty;
        StatusMessage = "Jogo atualizado";
        LatestVersionLabel = versionLabel;

        await _settings.SaveInstallMetadataAsync(versionLabel).ConfigureAwait(true);
        NotifyChanged();
        await _alert.ShowSuccess("Atualizacao concluida", $"Versao {versionLabel} instalada com sucesso.")
            .ConfigureAwait(true);
    }

    public async Task EnsureLoadedAsync()
    {
        if (_settingsLoaded)
            return;

        await _settings.LoadAsync().ConfigureAwait(true);
        _settingsLoaded = true;
        RefreshInstallState(false);
    }

    public async Task FailUpdateAsync(string message)
    {
        IsUpdating = false;
        CurrentFileName = string.Empty;
        StatusMessage = IsInstalled ? "Falha ao atualizar. Jogo instalado" : "Falha ao atualizar";
        NotifyChanged();
        await _alert.ShowError("Atualizacao", message).ConfigureAwait(true);
    }

    public async Task<bool> LaunchGameAsync()
    {
        await EnsureLoadedAsync().ConfigureAwait(true);

        string? executablePath = FindGameExecutable();
        if (string.IsNullOrWhiteSpace(executablePath))
        {
            await _alert.ShowError(
                    "Iniciar jogo",
                    "Nao foi possivel localizar o executavel do jogo. Instale, atualize ou ajuste a pasta nas configuracoes.")
                .ConfigureAwait(true);
            RefreshInstallState();
            return false;
        }

        try
        {
#if WINDOWS
            Process.Start(
                new ProcessStartInfo(executablePath)
                {
                    UseShellExecute = true,
                    WorkingDirectory = Path.GetDirectoryName(executablePath) ?? _settings.InstallDirectory,
                });
#elif MACCATALYST || MACOS
            Process.Start("open", $"\"{executablePath}\"");
#else
            await _alert.ShowError("Iniciar jogo", "Iniciar o jogo pelo launcher nao esta disponivel nesta plataforma.")
                .ConfigureAwait(true);
            return false;
#endif
            StatusMessage = "Jogo iniciado";
            NotifyChanged();
            await _alert.ShowSuccess("Iniciar jogo", "O jogo foi iniciado.").ConfigureAwait(true);
            return true;
        }
        catch (Exception ex)
        {
            await _alert.ShowError("Iniciar jogo", "Falha ao iniciar o jogo: " + ex.Message).ConfigureAwait(true);
            return false;
        }
    }

    public async Task<bool> LoginAsync(LoginModel model, CancellationToken cancellationToken = default)
    {
        try
        {
            if (!Uri.TryCreate(_settings.GatewayBaseUrl, UriKind.Absolute, out Uri? gatewayBase))
            {
                await _alert.ShowError("Login", "Gateway nao configurado. Verifique as configuracoes.")
                    .ConfigureAwait(true);
                return false;
            }

            HttpClientHandler handler = new HttpClientHandler();
#if DEBUG
            handler.ServerCertificateCustomValidationCallback = (_, _, _, _) => true;
#endif
            using HttpClient client = new HttpClient(handler);

            string jsonContent = JsonConvert.SerializeObject(model);
            StringContent httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync(
                new Uri(gatewayBase, "api/Account/Login"),
                httpContent,
                cancellationToken)
                .ConfigureAwait(true);

            if (!response.IsSuccessStatusCode)
            {
                await _alert.ShowError("Login", "Credenciais invalidas ou servidor indisponivel.")
                    .ConfigureAwait(true);
                return false;
            }

            string json = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(true);
            DefaultResponse? defaultResponse = JsonConvert.DeserializeObject<DefaultResponse>(json);

            if (defaultResponse != null && IsSuccessStatus(defaultResponse.HttpStatus))
            {
                try
                {
                    UserModel? user = defaultResponse.ObjectResponse != null
                        ? JsonConvert.DeserializeObject<UserModel>(defaultResponse.ObjectResponse.ToString())
                        : null;

                    if (user == null)
                    {
                        await _alert.ShowError("Login", "Falha ao processar a resposta do servidor.")
                            .ConfigureAwait(true);
                        return false;
                    }

                    Username = user.UserName ?? user.Email;
                    Token = user.Token;
                    IsLoggedIn = true;
                    StatusMessage = IsInstalled ? "Jogo pronto" : "Login realizado";
                    await SaveTokenAsync(Token ?? string.Empty).ConfigureAwait(true);
                    NotifyChanged();
                    await _alert.ShowSuccess("Login", "Login realizado com sucesso.").ConfigureAwait(true);
                    return true;
                }
                catch (Exception ex)
                {
                    await _alert.ShowError("Login", "Falha ao processar a resposta do servidor: " + ex.Message)
                        .ConfigureAwait(true);
                    return false;
                }
            }

            await _alert.ShowError("Login", defaultResponse?.Message ?? "Credenciais invalidas ou servidor indisponivel.")
                .ConfigureAwait(true);
            return false;
        }
        catch (Exception ex)
        {
            await _alert.ShowError("Login", "Falha ao conectar ao servidor: " + ex.Message).ConfigureAwait(true);
            return false;
        }
    }

    public async Task Logout()
    {
        Username = null;
        Token = null;
        IsLoggedIn = false;
        StatusMessage = IsInstalled ? "Jogo pronto" : "Sessao encerrada";
        await ClearTokenAsync().ConfigureAwait(true);
        NotifyChanged();
        await _alert.ShowSuccess("Logout", "Sessao encerrada.").ConfigureAwait(true);
    }

    public async Task<bool> OpenInstallDirectoryAsync()
    {
        await EnsureLoadedAsync().ConfigureAwait(true);

        if (!InstallDirectoryExists)
        {
            await _alert.ShowError("Pasta do jogo", "A pasta de instalacao ainda nao existe.").ConfigureAwait(true);
            return false;
        }

        try
        {
#if WINDOWS
            Process.Start(
                new ProcessStartInfo("explorer.exe", $"\"{_settings.InstallDirectory}\"")
                {
                    UseShellExecute = true,
                });
#elif MACCATALYST || MACOS
            Process.Start("open", $"\"{_settings.InstallDirectory}\"");
#else
            await _alert.ShowError("Pasta do jogo", "Abrir a pasta nao esta disponivel nesta plataforma.")
                .ConfigureAwait(true);
            return false;
#endif
            return true;
        }
        catch (Exception ex)
        {
            await _alert.ShowError("Pasta do jogo", "Falha ao abrir a pasta: " + ex.Message).ConfigureAwait(true);
            return false;
        }
    }

    public void RefreshInstallState(bool notify = true)
    {
        if (!IsUpdating)
            StatusMessage = IsInstalled ? "Jogo pronto" : "Instalacao pendente";

        if (notify)
            NotifyChanged();
    }

    public void ReportUpdate(UpdateProgressInfo info)
    {
        if (!IsUpdating)
            return;

        UpdateProgress = Math.Clamp(info.Percentage, 0, 100);
        CompletedFiles = info.CompletedFiles;
        TotalFiles = info.TotalFiles;
        CurrentFileName = info.CurrentFileName;
        StatusMessage = string.IsNullOrWhiteSpace(info.Message) ? StatusMessage : info.Message;
        NotifyChanged();
    }

    public void RequestStopUpdate()
    {
        if (!IsUpdating)
            return;

        _updateCts?.Cancel();
        StatusMessage = "Cancelando atualizacao";
        NotifyChanged();
    }

    public void SetLatestVersion(string versionLabel, int availableFiles)
    {
        LatestVersionLabel = string.IsNullOrWhiteSpace(versionLabel) ? "Nao verificada" : versionLabel;
        AvailableFiles = availableFiles;
        LastVersionCheckUtc = DateTime.UtcNow;
        StatusMessage = IsInstalled && string.Equals(InstalledVersionLabel, LatestVersionLabel, StringComparison.OrdinalIgnoreCase)
            ? "Jogo atualizado"
            : "Atualizacao disponivel";
        NotifyChanged();
    }

    private async Task ClearTokenAsync()
    {
        try
        {
            await Microsoft.Maui.Storage.SecureStorage.Default.SetAsync("ROH_LAUNCHER_TOKEN", string.Empty)
                .ConfigureAwait(true);
        }
        catch
        {
        }
    }

    private string? FindGameExecutable()
    {
        if (!InstallDirectoryExists)
            return null;

#if WINDOWS
        string[] preferredNames =
        [
            _settings.GameExecutableName,
            "ReignOfHumanae.exe",
            "Reign Of Humanae.exe",
            "ROH.exe",
        ];

        foreach (string preferredName in preferredNames.Where(name => !string.IsNullOrWhiteSpace(name)).Distinct(StringComparer.OrdinalIgnoreCase))
        {
            string path = Path.Combine(_settings.InstallDirectory, preferredName);
            if (File.Exists(path))
                return path;
        }

        return Directory
            .EnumerateFiles(_settings.InstallDirectory, "*.exe", SearchOption.TopDirectoryOnly)
            .FirstOrDefault();
#elif MACCATALYST || MACOS
        string[] preferredNames =
        [
            "ReignOfHumanae.app",
            "ROH.app",
        ];

        foreach (string preferredName in preferredNames)
        {
            string path = Path.Combine(_settings.InstallDirectory, preferredName);
            if (Directory.Exists(path))
                return path;
        }

        return Directory
            .EnumerateDirectories(_settings.InstallDirectory, "*.app", SearchOption.TopDirectoryOnly)
            .FirstOrDefault();
#else
        return null;
#endif
    }

    private async Task InitializeAsync()
    {
        await _settings.LoadAsync().ConfigureAwait(true);
        _settingsLoaded = true;
        await LoadTokenAsync().ConfigureAwait(true);
        RefreshInstallState();
    }

    private async Task LoadTokenAsync()
    {
        try
        {
            string? token = await Microsoft.Maui.Storage.SecureStorage.Default.GetAsync("ROH_LAUNCHER_TOKEN")
                .ConfigureAwait(true);
            if (!string.IsNullOrWhiteSpace(token))
            {
                Token = token;
                IsLoggedIn = true;
            }
        }
        catch
        {
        }
    }

    private static bool IsSuccessStatus(HttpStatusCode statusCode) => (int)statusCode >= 200 && (int)statusCode < 300;

    private void NotifyChanged() => OnChange?.Invoke();

    private async Task SaveTokenAsync(string token)
    {
        try
        {
            await Microsoft.Maui.Storage.SecureStorage.Default.SetAsync("ROH_LAUNCHER_TOKEN", token)
                .ConfigureAwait(true);
        }
        catch
        {
        }
    }
}
