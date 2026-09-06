#if MACCATALYST || __MACCATALYST__ || __MACOS__ || MACOS
using ROH.Launcher.Services;

namespace ROH.Launcher.Platforms.MacCatalyst;

// For MacCatalyst and macOS fallback to FilePicker-based directory selection
public class FolderPickerImplementation : IFolderPicker
{
    public async Task<string?> PickFolderAsync()
    {
        try
        {
            var result = await Microsoft.Maui.Storage.FilePicker.Default.PickAsync().ConfigureAwait(true);
            if (result == null)
                return null;

            if (!string.IsNullOrWhiteSpace(result.FullPath))
            {
                var dir = System.IO.Path.GetDirectoryName(result.FullPath);
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
#endif
