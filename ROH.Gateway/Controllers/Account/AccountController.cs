using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using ROH.StandardModels.Account;

namespace ROH.Gateway.Controllers.Account;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AccountController : ControllerBase
{
    private readonly Utils.ApiConfiguration.Api _api = new();

    [HttpPost("CreateNewUser")]
    public async Task<IActionResult> CreateNewUser(UserModel userModel) => Ok(await _api.Post(Utils.ApiConfiguration.Api.Services.CreateNewUser, userModel));

    [HttpGet("FindUserByEmail")]
    public async Task<IActionResult> FindUserByEmail(string email) => Ok(await _api.Get(Utils.ApiConfiguration.Api.Services.FindUserByEmail, new { Email = email }));

    [HttpGet("FindUserByUserName")]
    public async Task<IActionResult> FindUserByUserName(string userName) => Ok(await _api.Get(Utils.ApiConfiguration.Api.Services.FindUserByUserName, new { UserName = userName }));

    [HttpGet("GetUserByGuid")]
    public async Task<IActionResult> GetUserByGuid(Guid guid) => Ok(await _api.Get(Utils.ApiConfiguration.Api.Services.GetUserByGuid, new { Guid = guid }));

    [HttpGet("GetAccountByUserGuid")]
    public async Task<IActionResult> GetAccountByUserGuid(Guid guid) => Ok(await _api.Get(Utils.ApiConfiguration.Api.Services.GetAccountByUserGuid, new { Guid = guid }));

    [HttpPut("UpdateAccount")]
    public async Task<IActionResult> UpdateAccount(AccountModel model) => Ok(await _api.Update(Utils.ApiConfiguration.Api.Services.UpdateAccount, model));

    [AllowAnonymous]
    [HttpPut("Login")]
    public async Task<IActionResult> Login(LoginModel model) => Ok(await _api.Post(Utils.ApiConfiguration.Api.Services.Login, model));
}
