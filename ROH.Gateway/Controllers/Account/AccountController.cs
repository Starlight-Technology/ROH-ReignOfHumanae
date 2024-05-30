using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using ROH.StandardModels.Account;

namespace ROH.Gateway.Controllers.Account;
[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly Utils.ApiConfiguration.Api _api = new();

    [HttpPost("CreateNewUser")]
    public async Task<IActionResult> CreateNewUser(UserModel userModel) => Ok(await _api.Post(Utils.ApiConfiguration.Api.Services.CreateNewUser, userModel));

    [HttpGet("FindUserByEmail")]
    public async Task<IActionResult> FindUserByEmail(string email) => Ok(await _api.Get(Utils.ApiConfiguration.Api.Services.FindUserByEmail, email));

    [HttpGet("FindUserByUserName")]
    public async Task<IActionResult> FindUserByUserName(string userName) => Ok(await _api.Get(Utils.ApiConfiguration.Api.Services.FindUserByUserName, userName));

    [HttpGet("GetUserByGuid")]
    public async Task<IActionResult> GetUserByGuid(Guid guid) => Ok(await _api.Get(Utils.ApiConfiguration.Api.Services.GetUserByGuid, guid));

    [HttpGet("GetAccounByUserGuid")]
    public async Task<IActionResult> GetAccounByUserGuid(Guid guid) => Ok(await _api.Get(Utils.ApiConfiguration.Api.Services.GetAccounByUserGuid, guid));

    [HttpPut("UpdateAccount")]
    public async Task<IActionResult> UpdateAccount(AccountModel model) => Ok(await _api.Update(Utils.ApiConfiguration.Api.Services.UpdateAccount, model));
}
