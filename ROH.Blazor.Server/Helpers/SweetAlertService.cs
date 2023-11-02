// Ignore Spelling: js

using Microsoft.JSInterop;

using ROH.Blazor.Server.Helpers.Types;

namespace ROH.Blazor.Server.Helpers
{
    public class SweetAlertService
    {
        private readonly IJSRuntime _jsRuntime;

        public SweetAlertService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task Show(string title, string message, SweetAlertType type)
        {
            await _jsRuntime.InvokeVoidAsync("window.sweetalertInterop.showSweetAlert", title, message, type.ToString().ToLower());
        }
    }
}
