using Microsoft.AspNetCore.Mvc;

using ROH.StandardModels.Version;

namespace ROH.Gateway.Controllers.Version
{
    [Route("api/[controller]")]
    [ApiController]
    public class VersionController : ControllerBase
    {
        private readonly Utils.ApiConfiguration.Api _api = new();

        [HttpGet]
        public async Task<IActionResult> GetCurrentVersion() => Ok(await _api.Get(Utils.ApiConfiguration.Api.Services.GetCurrentVersion, new List<Utils.ApiConfiguration.ApiParameters>()));

        [HttpPost]
        public async Task<IActionResult> CreateNewVersion(GameVersionModel model) => Ok(await _api.Post(Utils.ApiConfiguration.Api.Services.CreateNewVersion, model));
    }
}