using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ROH.Gateway.Controllers.Version
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly Utils.ApiConfiguration.Api _api = new();


        [HttpPost("UploadFile")]
        public async Task<IActionResult> UploadFile() => Ok(await _api.Post(Utils.ApiConfiguration.Api.Services.UploadFile, new ()));
    }
}
