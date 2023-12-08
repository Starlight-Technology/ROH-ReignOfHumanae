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
        public async Task<IActionResult> GetCurrentVersion()
        {
            return Ok(await _api.Get(Utils.ApiConfiguration.Api.Services.GetCurrentVersion, []));
        }

        [HttpGet("GetAllVersionsPaginated")]
        public async Task<IActionResult> GetAllVersionsPaginated(int page = 1, int take = 10)
        {
            return Ok(await _api.Get(Utils.ApiConfiguration.Api.Services.GetAllVersionsPaginated, [
            new Utils.ApiConfiguration.ApiParameters() { Name = "page", Value = page.ToString() },
                new Utils.ApiConfiguration.ApiParameters() { Name = "take", Value = take.ToString() },
            ]));
        }

        [HttpGet("GetAllReleasedVersionsPaginated")]
        public async Task<IActionResult> GetAllReleasedVersionsPaginated(int page = 1, int take = 10)
        {
            return Ok(await _api.Get(Utils.ApiConfiguration.Api.Services.GetAllReleasedVersionsPaginated, [
            new Utils.ApiConfiguration.ApiParameters() { Name = "page", Value = page.ToString() },
                new Utils.ApiConfiguration.ApiParameters() { Name = "take", Value = take.ToString() },
            ]));
        }

        [HttpGet("GetVersionDetails")]
        public async Task<IActionResult> GetVersionDetails(Guid guid)
        {
            return Ok(await _api.Get(Utils.ApiConfiguration.Api.Services.GetVersionDetails, [
            new Utils.ApiConfiguration.ApiParameters() { Name = "guid", Value = guid.ToString() },
            ]));
        }

        [HttpPost("CreateNewVersion")]
        public async Task<IActionResult> CreateNewVersion(GameVersionModel model)
        {
            return Ok(await _api.Post(Utils.ApiConfiguration.Api.Services.CreateNewVersion, model));
        }
    }
}