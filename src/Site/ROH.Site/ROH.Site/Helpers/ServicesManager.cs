//-----------------------------------------------------------------------
// <copyright file="ServicesManager.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using ROH.Site.Api;
using ROH.Site.Interfaces.Api;
using ROH.Site.Interfaces.Helpers;

namespace ROH.Site.Helpers;

public class ServicesManager
{
    public void ConfigureServices(IServiceCollection services)
    {
        _ = services.AddScoped<IVersionService, VersionService>();
        _ = services.AddScoped<IVersionFileService, VersionFileService>();
        _ = services.AddScoped<ISweetAlertService, SweetAlertService>();
        _ = services.AddScoped<IDownloadFileService, DownloadFileService>();
        _ = services.AddScoped<IAccountService, AccountService>();
        _ = services.AddScoped<ICustomAuthenticationStateProvider, CustomAuthenticationStateProvider>();
    }
}