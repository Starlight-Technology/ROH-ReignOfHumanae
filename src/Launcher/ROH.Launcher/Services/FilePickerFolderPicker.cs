using System.Threading.Tasks;
using ROH.Launcher.Services;

namespace ROH.Launcher.Services
{
    // fallback implementation using FilePicker
    public class FilePickerFolderPicker : IFolderPicker
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
}
