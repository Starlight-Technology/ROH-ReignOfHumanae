//-----------------------------------------------------------------------
// <copyright file="VersionController.cs" company="Starlight-Technology">
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
public class VersionController : ControllerBase
{
    private readonly Api _api = new();

    [HttpPost("CreateNewVersion")]
    public async Task<IActionResult> CreateNewVersionAsync(GameVersionModel model, CancellationToken cancellationToken = default)
    {
        using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        cts.CancelAfter(TimeSpan.FromMinutes(2));
        try
        {
            var result = await _api.PostAsync(Api.Services.CreateNewVersion, model, cts.Token).ConfigureAwait(true);
            return Ok(result);
        }
        catch (OperationCanceledException)
        {
            return StatusCode(408, "The request timed out.");
        }
    }

    [HttpGet("GetAllReleasedVersionsPaginated")]
    public async Task<IActionResult> GetAllReleasedVersionsPaginatedAsync(int page = 1, int take = 10, CancellationToken cancellationToken = default)
    {
        using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        cts.CancelAfter(TimeSpan.FromMinutes(2));
        try
        {
            var result = await _api.GetAsync(Api.Services.GetAllReleasedVersionsPaginated, new { Page = page, Take = take }, cts.Token).ConfigureAwait(true);
            return Ok(result);
        }
        catch (OperationCanceledException)
        {
            return StatusCode(408, "The request timed out.");
        }
    }

    [HttpGet("GetAllVersionsPaginated")]
    public async Task<IActionResult> GetAllVersionsPaginatedAsync(int page = 1, int take = 10, CancellationToken cancellationToken = default)
    {
        using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        cts.CancelAfter(TimeSpan.FromMinutes(2));
        try
        {
            var result = await _api.GetAsync<object>(Api.Services.GetAllVersionsPaginated, new { Page = page, Take = take }, cts.Token).ConfigureAwait(true);
            return Ok(result);
        }
        catch (OperationCanceledException)
        {
            return StatusCode(408, "The request timed out.");
        }
    }

    [HttpGet("GetCurrentVersion")]
    public async Task<IActionResult> GetCurrentVersionAsync(CancellationToken cancellationToken = default)
    {
        using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        cts.CancelAfter(TimeSpan.FromMinutes(2));
        try
        {
            var result = await _api.GetAsync<object?>(Api.Services.GetCurrentVersion, null, cts.Token).ConfigureAwait(true);
            return Ok(result);
        }
        catch (OperationCanceledException)
        {
            return StatusCode(408, "The request timed out.");
        }
    }

    [HttpGet("GetVersionDetails")]
    public async Task<IActionResult> GetVersionDetailsAsync(Guid guid, CancellationToken cancellationToken = default)
    {
        using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        cts.CancelAfter(TimeSpan.FromMinutes(2));
        try
        {
            var result = await _api.GetAsync(Api.Services.GetVersionDetails, new { Guid = guid }, cts.Token).ConfigureAwait(true);
            return Ok(result);
        }
        catch (OperationCanceledException)
        {
            return StatusCode(408, "The request timed out.");
        }
    }

    [HttpPut("ReleaseVersion")]
    public async Task<IActionResult> ReleaseVersionAsync([FromBody] GameVersionModel gameVersion, CancellationToken cancellationToken = default)
    {
        using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        cts.CancelAfter(TimeSpan.FromMinutes(2));
        try
        {
            var result = await _api.UpdateAsync(Api.Services.ReleaseVersion, gameVersion, cts.Token).ConfigureAwait(true);
            return Ok(result);
        }
        catch (OperationCanceledException)
        {
            return StatusCode(408, "The request timed out.");
        }
    }

}