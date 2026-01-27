//-----------------------------------------------------------------------
// <copyright file="ISweetAlertService.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
// Ignore Spelling: js

using ROH.Site.Helpers.Types;
using ROH.StandardModels.Response;

namespace ROH.Site.Interfaces.Helpers;

public interface ISweetAlertService
{
    Task Show(string title, string message, SweetAlertType type);

    Task ShowResponse(DefaultResponse response);
}