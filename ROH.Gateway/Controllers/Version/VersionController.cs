using Microsoft.AspNetCore.Mvc;

using ROH.StandardModels.Version;

namespace ROH.Gateway.Controllers.Version
{
    [Route("api/[controller]")]
    [ApiController]
    public class VersionController : ControllerBase
    {
        private readonly Utils.ApiConfiguration.Api _api = new();

        [HttpGet("GetCurrentVersion")]
        public async Task<IActionResult> GetCurrentVersion() => Ok(await _api.Get(Utils.ApiConfiguration.Api.Services.GetCurrentVersion, new List<Utils.ApiConfiguration.ApiParameters>()));

        [HttpGet("GetAllVersionsPaginated")]
        public async Task<IActionResult> GetAllVersionsPaginated(int page = 1, int take = 10) => Ok(await _api.Get(Utils.ApiConfiguration.Api.Services.GetAllVersionsPaginated, new List<Utils.ApiConfiguration.ApiParameters>() {
            new Utils.ApiConfiguration.ApiParameters() {Name="page", Value= page.ToString() },
            new Utils.ApiConfiguration.ApiParameters() {Name="take", Value= take.ToString() },
        }));

        [HttpPost("CreateNewVersion")]
        public async Task<IActionResult> CreateNewVersion(GameVersionModel model) => Ok(await _api.Post(Utils.ApiConfiguration.Api.Services.CreateNewVersion, model));
    }
}