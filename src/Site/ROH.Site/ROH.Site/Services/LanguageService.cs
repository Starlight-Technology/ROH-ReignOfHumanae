using Blazored.LocalStorage;

using System.Globalization;

namespace ROH.Site.Services;

public class LanguageService(ILocalStorageService localStorage)
{
    private const string StorageKey = "lang";

    public async Task<string> GetCurrentCultureAsync()
    {
        string? lang = await localStorage.GetItemAsStringAsync(StorageKey);
        return lang?.Trim('"') ?? "en";
    }

    public async Task SetCurrentCultureAsync(string culture)
    {
        await localStorage.SetItemAsync(StorageKey, culture);
        CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(culture);
        CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo(culture);
    }
}
