using Microsoft.AspNetCore.Mvc;

using ROH.StandardModels.Version;
using ROH.Utils.ApiConfiguration;

namespace ROH.Gateway.Controllers.Version
{
    [Route("api/[controller]")]
    [ApiController]
    public class VersionFileController : ControllerBase
    {
        private readonly Utils.ApiConfiguration.Api _api = new();

        [HttpPost("UploadFile")]
        public async Task<IActionResult> UploadFile(GameVersionFileModel file) => Ok(await _api.Post(Api.Services.UploadVersionFile, file));

        [HttpPost("GetAllVersionFiles")]
        public async Task<IActionResult> GetAllVersionFiles(string versionGuid) => Ok(await _api.Get(Api.Services.GetAllVersionFiles, new { VersionGuid = versionGuid }));
    }
}