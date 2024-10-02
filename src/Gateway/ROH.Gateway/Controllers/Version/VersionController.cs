using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using ROH.StandardModels.Version;

namespace ROH.Gateway.Controllers.Version;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class VersionController : ControllerBase
{
    private readonly Utils.ApiConfiguration.Api _api = new();

    [HttpGet("GetCurrentVersion")]
    public async Task<IActionResult> GetCurrentVersion() => Ok(await _api.GetAsync<object?>(Utils.ApiConfiguration.Api.Services.GetCurrentVersion, null));

    [HttpGet("GetAllVersionsPaginated")]
    public async Task<IActionResult> GetAllVersionsPaginated(int page = 1, int take = 10) => Ok(await _api.GetAsync<object>(Utils.ApiConfiguration.Api.Services.GetAllVersionsPaginated, new { Page = page, Take = take }));

    [HttpGet("GetAllReleasedVersionsPaginated")]
    public async Task<IActionResult> GetAllReleasedVersionsPaginated(int page = 1, int take = 10) => Ok(await _api.GetAsync(Utils.ApiConfiguration.Api.Services.GetAllReleasedVersionsPaginated, new { Page = page, Take = take }));

    [HttpGet("GetVersionDetails")]
    public async Task<IActionResult> GetVersionDetails(Guid guid) => Ok(await _api.GetAsync(Utils.ApiConfiguration.Api.Services.GetVersionDetails, new { Guid = guid }));

    [HttpPost("CreateNewVersion")]
    public async Task<IActionResult> CreateNewVersion(GameVersionModel model) => Ok(await _api.Post(Utils.ApiConfiguration.Api.Services.CreateNewVersion, model));

    [HttpPut("ReleaseVersion")]
    public async Task<IActionResult> ReleaseVersion([FromBody] GameVersionModel gameVersion) => Ok(await _api.Update(Utils.ApiConfiguration.Api.Services.ReleaseVersion, gameVersion));
}