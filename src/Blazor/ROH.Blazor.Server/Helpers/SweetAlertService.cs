﻿// Ignore Spelling: js

using Microsoft.JSInterop;

using ROH.Blazor.Server.Helpers.Types;
using ROH.Blazor.Server.Interfaces.Helpers;
using ROH.StandardModels.Response;
using ROH.Utils.Helpers;

namespace ROH.Blazor.Server.Helpers;

public class SweetAlertService(IJSRuntime _jsRuntime) : ISweetAlertService
{
    public async Task Show(string title, string message, SweetAlertType type) => await _jsRuntime.InvokeVoidAsync("window.sweetalertInterop.showSweetAlert", title, message, type.ToString().ToLower());

    public async Task ShowResponse(DefaultResponse response)
    {
        SweetAlertType type = SweetAlertType.Error;

        if (response.HttpStatus.IsSuccessStatusCode())
        {
            type = SweetAlertType.Success;
        }
        else if (response.HttpStatus.IsClientErrorStatusCode() || response.HttpStatus.IsServerErrorStatusCode())
        {
            type = SweetAlertType.Error;
        }

        await Show("", response.Message, type);
    }
}