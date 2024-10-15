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

using System.Threading;

namespace ROH.Gateway.Controllers.Version;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class VersionFileController : ControllerBase
{
    private readonly Api _api = new();
    [HttpGet("DownloadFile")]
    public async Task<IActionResult> DownloadFileAsync(string fileGuid, CancellationToken cancellationToken = default)
    {
        using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        cts.CancelAfter(TimeSpan.FromMinutes(2));
        try
        {
            var result = await _api.GetAsync(Api.Services.DownloadFile, new { FileGuid = fileGuid }, cts.Token).ConfigureAwait(true);
            return Ok(result);
        }
        catch (OperationCanceledException)
        {
            return StatusCode(408, "The request timed out.");
        }
    }

    [HttpGet("GetAllVersionFiles")]
    public async Task<IActionResult> GetAllVersionFilesAsync(string versionGuid, CancellationToken cancellationToken = default)
    {
        using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        cts.CancelAfter(TimeSpan.FromMinutes(2));
        try
        {
            var result = await _api.GetAsync(Api.Services.GetAllVersionFiles, new { VersionGuid = versionGuid }, cts.Token).ConfigureAwait(true);
            return Ok(result);
        }
        catch (OperationCanceledException)
        {
            return StatusCode(408, "The request timed out.");
        }
    }

    [HttpPost("UploadFile")]
    public async Task<IActionResult> UploadFileAsync(GameVersionFileModel file, CancellationToken cancellationToken = default)
    {
        using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        cts.CancelAfter(TimeSpan.FromMinutes(2));
        try
        {
            var result = await _api.PostAsync(Api.Services.UploadVersionFile, file, cts.Token).ConfigureAwait(true);
            return Ok(result);
        }
        catch (OperationCanceledException)
        {
            return StatusCode(408, "The request timed out.");
        }
    }

}