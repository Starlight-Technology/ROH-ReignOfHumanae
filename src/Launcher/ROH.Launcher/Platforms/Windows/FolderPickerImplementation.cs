#if WINDOWS
using ROH.Launcher.Services;

using Windows.Storage;
using Windows.Storage.Pickers;

namespace ROH.Launcher.Platforms.Windows;

public class FolderPickerImplementation : IFolderPicker
{
    public async Task<string?> PickFolderAsync()
    {
        // Try to obtain the MAUI window and get HWND
        try
        {
            var mauiApp = Microsoft.Maui.Controls.Application.Current;
            var mauiWindow = mauiApp?.Windows?.Count > 0 ? mauiApp.Windows[0].Handler.PlatformView as Microsoft.UI.Xaml.Window : null;
            var hwnd = mauiWindow != null ? WinRT.Interop.WindowNative.GetWindowHandle(mauiWindow) : nint.Zero;

            var picker = new FolderPicker();
            if (hwnd != nint.Zero)
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
#endif
