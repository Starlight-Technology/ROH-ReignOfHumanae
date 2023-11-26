// Ignore Spelling: js

using ROH.Blazor.Server.Helpers.Types;
using ROH.StandardModels.Response;

namespace ROH.Blazor.Server.Interfaces.Helpers
{
    public interface ISweetAlertService
    {
        Task Show(string title, string message, SweetAlertType type);

        Task ShowResponse(DefaultResponse response);
    }
}