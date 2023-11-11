using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using ROH.StandardModels.Version;

namespace ROH.Gateway.Controllers.Version
{
    [Route("api/[controller]")]
    [ApiController]
    public class VersionFileController : ControllerBase
    {
        private readonly Utils.ApiConfiguration.Api _api = new();


        [HttpPost("UploadFile")]
        public async Task<IActionResult> UploadFile(GameVersionFileModel file) => Ok(await _api.Post(Utils.ApiConfiguration.Api.Services.UploadVersionFile, file));
    }
}
