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
    public async Task<IActionResult> CreateNewVersion(GameVersionModel model) => Ok(
        await _api.Post(Api.Services.CreateNewVersion, model));

    [HttpGet("GetAllReleasedVersionsPaginated")]
    public async Task<IActionResult> GetAllReleasedVersionsPaginated(int page = 1, int take = 10) => Ok(
        await _api.GetAsync(Api.Services.GetAllReleasedVersionsPaginated, new { Page = page, Take = take }));

    [HttpGet("GetAllVersionsPaginated")]
    public async Task<IActionResult> GetAllVersionsPaginated(int page = 1, int take = 10) => Ok(
        await _api.GetAsync<object>(Api.Services.GetAllVersionsPaginated, new { Page = page, Take = take }));

    [HttpGet("GetCurrentVersion")]
    public async Task<IActionResult> GetCurrentVersion() => Ok(
        await _api.GetAsync<object?>(Api.Services.GetCurrentVersion, null));

    [HttpGet("GetVersionDetails")]
    public async Task<IActionResult> GetVersionDetails(Guid guid) => Ok(
        await _api.GetAsync(Api.Services.GetVersionDetails, new { Guid = guid }));

    [HttpPut("ReleaseVersion")]
    public async Task<IActionResult> ReleaseVersion([FromBody] GameVersionModel gameVersion) => Ok(
        await _api.Update(Api.Services.ReleaseVersion, gameVersion));
}