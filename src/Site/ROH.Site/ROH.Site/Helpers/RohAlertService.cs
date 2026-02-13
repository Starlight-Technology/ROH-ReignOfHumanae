using Microsoft.AspNetCore.Components;

using MudBlazor;

using ROH.Site.Components.Custom;
using ROH.Site.Helpers.Types;
using ROH.Site.Interfaces.Helpers;
using ROH.StandardModels.Response;
using ROH.Utils.Helpers;

using System.Threading.Tasks;


namespace ROH.Site.Helpers;

public class RohAlertService(IDialogService dialog,
                             ICustomAuthenticationStateProvider _authenticationStateProvider,
                             NavigationManager _navigation) : IRohAlertService
{
    public async Task<IDialogReference> Show(string title, string message, RohAlertType type)
    {
        var parameters = new DialogParameters
        {
            ["Title"] = title,
            ["Message"] = message,
            ["Type"] = type
        };

        return await dialog.ShowAsync<RohAlertDialog>(string.Empty, parameters);
    }

    public async Task<IDialogReference> ShowResponse(DefaultResponse response)
    {
        RohAlertType type = RohAlertType.Error;

        if (response.HttpStatus.IsSuccessStatusCode())
        {
            type = RohAlertType.Success;
        }
        else if (response.HttpStatus == System.Net.HttpStatusCode.Unauthorized)
        {
            type = RohAlertType.Warning;
            await _authenticationStateProvider.MarkUserAsLoggedOut().ConfigureAwait(false);
            _navigation.NavigateTo("/login");
        }
        else if (response.HttpStatus.IsClientErrorStatusCode() || response.HttpStatus.IsServerErrorStatusCode())
        {
            type = RohAlertType.Error;
        }

        return await Show(string.Empty, response.Message, type);
    }
}

