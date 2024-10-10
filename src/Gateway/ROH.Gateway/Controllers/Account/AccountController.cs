//-----------------------------------------------------------------------
// <copyright file="AccountController.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using ROH.StandardModels.Account;
using ROH.Utils.ApiConfiguration;

namespace ROH.Gateway.Controllers.Account;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AccountController : ControllerBase
{
    private readonly Api _api = new();

    [AllowAnonymous]
    [HttpPost("CreateNewUser")]
    public async Task<IActionResult> CreateNewUser(UserModel userModel) => Ok(
        await _api.Post(Api.Services.CreateNewUser, userModel));

    [HttpGet("FindUserByEmail")]
    public async Task<IActionResult> FindUserByEmail(string email) => Ok(
        await _api.GetAsync(Api.Services.FindUserByEmail, new { Email = email }));

    [HttpGet("FindUserByUserName")]
    public async Task<IActionResult> FindUserByUserName(string userName) => Ok(
        await _api.GetAsync(Api.Services.FindUserByUserName, new { UserName = userName }));

    [HttpGet("GetAccountByUserGuid")]
    public async Task<IActionResult> GetAccountByUserGuid(Guid guid) => Ok(
        await _api.GetAsync(Api.Services.GetAccountByUserGuid, new { Guid = guid }));

    [HttpGet("GetUserByGuid")]
    public async Task<IActionResult> GetUserByGuid(Guid guid) => Ok(
        await _api.GetAsync(Api.Services.GetUserByGuid, new { Guid = guid }));

    [AllowAnonymous]
    [HttpPost("Login")]
    public async Task<IActionResult> Login(LoginModel model) => Ok(await _api.Post(Api.Services.Login, model));

    [HttpPut("UpdateAccount")]
    public async Task<IActionResult> UpdateAccount(AccountModel model) => Ok(
        await _api.Update(Api.Services.UpdateAccount, model));
}
