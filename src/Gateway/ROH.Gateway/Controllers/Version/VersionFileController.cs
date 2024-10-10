//-----------------------------------------------------------------------
// <copyright file="VersionFileController.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using ROH.StandardModels.Version;
using ROH.Utils.ApiConfiguration;

namespace ROH.Gateway.Controllers.Version;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class VersionFileController : ControllerBase
{
    private readonly Api _api = new();

    [HttpGet("DownloadFile")]
    public async Task<IActionResult> DownloadFile(string fileGuid) => Ok(
        await _api.GetAsync(Api.Services.DownloadFile, new { FileGuid = fileGuid }));

    [HttpGet("GetAllVersionFiles")]
    public async Task<IActionResult> GetAllVersionFiles(string versionGuid) => Ok(
        await _api.GetAsync(Api.Services.GetAllVersionFiles, new { VersionGuid = versionGuid }));

    [HttpPost("UploadFile")]
    public async Task<IActionResult> UploadFile(GameVersionFileModel file) => Ok(
        await _api.Post(Api.Services.UploadVersionFile, file));
}