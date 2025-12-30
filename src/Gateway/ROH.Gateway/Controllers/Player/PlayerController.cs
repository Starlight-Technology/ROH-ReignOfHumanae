using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using ROH.StandardModels.Character;
using ROH.Utils.ApiConfiguration;

namespace ROH.Gateway.Controllers.Player;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class PlayerController : ControllerBase
{
    private readonly Api _api = new();

    [HttpGet("GetAllCharacters")]
    public async Task<IActionResult> GetAllCharacters(Guid accountGuid, CancellationToken token = default)
    {
        using var cts = CancellationTokenSource.CreateLinkedTokenSource(token);
        cts.CancelAfter(TimeSpan.FromMinutes(2));
        try
        {
            var response = await _api.GetAsync(Api.Services.GetAccountCaracters, new { accountGuid }, cts.Token)
                .ConfigureAwait(true);
            return Ok(response);
        }
        catch (OperationCanceledException)
        {
            return StatusCode(408, "The request timed out.");
        }
    }

    [HttpGet("GetCharacter")]
    public async Task<IActionResult> GetCharacter(Guid guid, CancellationToken token = default)
    {
        using var cts = CancellationTokenSource.CreateLinkedTokenSource(token);
        cts.CancelAfter(TimeSpan.FromMinutes(2));
        try
        {
            var response = await _api.GetAsync(Api.Services.GetCharacter, new { characterGuid = guid }, cts.Token)
                .ConfigureAwait(true);
            return Ok(response);
        }
        catch (OperationCanceledException)
        {
            return StatusCode(408, "The request timed out.");
        }
    }

    [HttpPost("CreateCharacter")]
    public async Task<IActionResult> CreateCharacter(CharacterModel model, CancellationToken token = default)
    {
        using var cts = CancellationTokenSource.CreateLinkedTokenSource(token);
        cts.CancelAfter(TimeSpan.FromMinutes(2));
        try
        {
            var response = await _api.PostAsync(Api.Services.CreateCharacter, model, cts.Token)
                .ConfigureAwait(true);
            return Ok(response);
        }
        catch (OperationCanceledException)
        {
            return StatusCode(408, "The request timed out.");
        }
    }
}