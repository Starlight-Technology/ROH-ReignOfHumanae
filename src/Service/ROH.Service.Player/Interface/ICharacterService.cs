//-----------------------------------------------------------------------
// <copyright file="ICharacterService.cs" company="Starlight-Technology">
//     Author:  
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using ROH.StandardModels.Character;
using ROH.StandardModels.Response;

namespace ROH.Service.Player.Interface;

public interface ICharacterService
{
    Task<DefaultResponse> AddCharacterAsync(CharacterModel model, CancellationToken token = default);

    Task<DefaultResponse> DeleteCharacterAsync(Guid id, CancellationToken token = default);

    Task<DefaultResponse> GetAllCharactersAsync(Guid accountGuid, CancellationToken token = default);

    Task<DefaultResponse> GetCharacterByGuidAsync(Guid guid, CancellationToken token = default);

    Task<DefaultResponse> UpdateCharacterAsync(CharacterModel model, CancellationToken token = default);
}
