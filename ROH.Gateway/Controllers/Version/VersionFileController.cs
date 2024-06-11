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
    private readonly Utils.ApiConfiguration.Api _api = new();

    [HttpGet("DownloadFile")]
    public async Task<IActionResult> DownloadFile(string fileGuid) => Ok(await _api.Get(Api.Services.DownloadFile, new { FileGuid = fileGuid }));

    [HttpPost("UploadFile")]
    public async Task<IActionResult> UploadFile(GameVersionFileModel file) => Ok(await _api.Post(Api.Services.UploadVersionFile, file));

    [HttpGet("GetAllVersionFiles")]
    public async Task<IActionResult> GetAllVersionFiles(string versionGuid) => Ok(await _api.Get(Api.Services.GetAllVersionFiles, new { VersionGuid = versionGuid }));
}