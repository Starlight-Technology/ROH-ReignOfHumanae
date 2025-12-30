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
    readonly Api _api = new();

    [AllowAnonymous]
    [HttpPost("CreateNewUser")]
    public async Task<IActionResult> CreateNewUserAsync(
        UserModel userModel,
        CancellationToken cancellationToken = default)
    {
        using CancellationTokenSource cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        cts.CancelAfter(TimeSpan.FromMinutes(2));
        try
        {
            return Ok(await _api.PostAsync(Api.Services.CreateNewUser, userModel, cts.Token).ConfigureAwait(true));
        }
        catch (OperationCanceledException)
        {
            return StatusCode(408, "The request timed out.");
        }
    }

    [HttpGet("FindUserByEmail")]
    public async Task<IActionResult> FindUserByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        using CancellationTokenSource cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        cts.CancelAfter(TimeSpan.FromMinutes(2));
        try
        {
            return Ok(
                await _api.GetAsync(Api.Services.FindUserByEmail, new { Email = email }, cts.Token).ConfigureAwait(true));
        }
        catch (OperationCanceledException)
        {
            return StatusCode(408, "The request timed out.");
        }
    }

    [HttpGet("FindUserByUserName")]
    public async Task<IActionResult> FindUserByUserNameAsync(
        string userName,
        CancellationToken cancellationToken = default)
    {
        using CancellationTokenSource cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        cts.CancelAfter(TimeSpan.FromMinutes(2));
        try
        {
            return Ok(
                await _api.GetAsync(Api.Services.FindUserByUserName, new { UserName = userName }, cts.Token)
                    .ConfigureAwait(true));
        }
        catch (OperationCanceledException)
        {
            return StatusCode(408, "The request timed out.");
        }
    }

    [HttpGet("GetAccountByUserGuid")]
    public async Task<IActionResult> GetAccountByUserGuidAsync(Guid guid, CancellationToken cancellationToken = default)
    {
        using CancellationTokenSource cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        cts.CancelAfter(TimeSpan.FromMinutes(2));
        try
        {
            return Ok(
                await _api.GetAsync(Api.Services.GetAccountByUserGuid, new { Guid = guid }, cts.Token)
                    .ConfigureAwait(true));
        }
        catch (OperationCanceledException)
        {
            return StatusCode(408, "The request timed out.");
        }
    }

    [HttpGet("GetUserByGuid")]
    public async Task<IActionResult> GetUserByGuidAsync(Guid guid, CancellationToken cancellationToken = default)
    {
        using CancellationTokenSource cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        cts.CancelAfter(TimeSpan.FromMinutes(2));
        try
        {
            return Ok(
                await _api.GetAsync(Api.Services.GetUserByGuid, new { Guid = guid }, cts.Token).ConfigureAwait(true));
        }
        catch (OperationCanceledException)
        {
            return StatusCode(408, "The request timed out.");
        }
    }

    [AllowAnonymous]
    [HttpPost("Login")]
    public async Task<IActionResult> LoginAsync(LoginModel model, CancellationToken cancellationToken = default)
    {
        using CancellationTokenSource cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        cts.CancelAfter(TimeSpan.FromMinutes(2));
        try
        {
            return Ok(await _api.PostAsync(Api.Services.Login, model, cts.Token).ConfigureAwait(true));
        }
        catch (OperationCanceledException)
        {
            return StatusCode(408, "The request timed out.");
        }
    }

    [HttpPut("UpdateAccount")]
    public async Task<IActionResult> UpdateAccountAsync(
        AccountModel model,
        CancellationToken cancellationToken = default)
    {
        using CancellationTokenSource cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        cts.CancelAfter(TimeSpan.FromMinutes(2));
        try
        {
            return Ok(await _api.UpdateAsync(Api.Services.UpdateAccount, model, cts.Token).ConfigureAwait(true));
        }
        catch (OperationCanceledException)
        {
            return StatusCode(408, "The request timed out.");
        }
    }
}