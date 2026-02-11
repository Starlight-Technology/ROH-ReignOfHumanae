using MudBlazor;

using ROH.Site.Helpers.Types;
using ROH.StandardModels.Response;


namespace ROH.Site.Interfaces.Helpers;

public interface IRohAlertService
{
    Task<IDialogReference> Show(string title, string message, RohAlertType type);

    Task<IDialogReference> ShowResponse(DefaultResponse response);
}

