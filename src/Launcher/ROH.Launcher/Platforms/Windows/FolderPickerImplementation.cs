#if WINDOWS
using System.Threading.Tasks;
using Windows.Storage.Pickers;
using Windows.Storage;
using Microsoft.Maui.Platform;
using ROH.Launcher.Services;

namespace ROH.Launcher.Platforms.Windows
{
    public class FolderPickerImplementation : IFolderPicker
    {
        public async Task<string?> PickFolderAsync()
        {
            // Try to obtain the MAUI window and get HWND
            try
            {
                var mauiApp = Microsoft.Maui.Controls.Application.Current;
                var mauiWindow = mauiApp?.Windows?.Count > 0 ? mauiApp.Windows[0].Handler.PlatformView as Microsoft.UI.Xaml.Window : null;
                var hwnd = mauiWindow != null ? WinRT.Interop.WindowNative.GetWindowHandle(mauiWindow) : System.IntPtr.Zero;

                var picker = new FolderPicker();
                if (hwnd != System.IntPtr.Zero)
                {
                    WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd);
                }

                picker.SuggestedStartLocation = PickerLocationId.ComputerFolder;
                picker.FileTypeFilter.Add("*");
                StorageFolder folder = await picker.PickSingleFolderAsync();
                return folder?.Path;
            }
            catch
            {
                return null;
            }
        }
    }
}
#endif
