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
        public async Task<IActionResult> UploadFile(GameVersionFileModel file) => Ok(await _api.Post(Utils.ApiConfiguration.Api.Services.UploadVersionFile, file));

        [HttpPost("GetAllVersionFiles")]
        public async Task<IActionResult> GetAllVersionFiles(string versionGuid) => Ok(await _api.Get(Utils.ApiConfiguration.Api.Services.GetAllVersionFiles, new List<ApiParameters>(){ new ApiParameters { Name= "versionGuid", Value = versionGuid } }));
    }
}