//-----------------------------------------------------------------------
// <copyright file="CharacterService.cs" company="Starlight-Technology">
//     Author:
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using AutoMapper;

using ROH.Context.Player.Entities.Characters;
using ROH.Context.Player.Interface;
using ROH.Service.Exception.Interface;
using ROH.Service.Player.Interface;
using ROH.StandardModels.Character;
using ROH.StandardModels.Response;

namespace ROH.Service.Player.Characters;

public class CharacterService(
    ICharacterRepository repository,
    IPositionRepository positionRepository,
    IMapper mapper,
    IExceptionHandler exceptionHandler) : ICharacterService
{
    public async Task<DefaultResponse> AddCharacterAsync(CharacterModel model, CancellationToken token = default)
    {
        try
        {
            Character entity = mapper.Map<Character>(model);

            await repository.AddCharacterAsync(entity, token).ConfigureAwait(true);

            return new DefaultResponse();
        }
        catch (System.Exception ex)
        {
            return exceptionHandler.HandleException(ex);
        }
    }

    public async Task<DefaultResponse> DeleteCharacterAsync(Guid id, CancellationToken token = default)
    {
        try
        {
            await repository.DeleteCharacterAsync(id).ConfigureAwait(true);

            return new DefaultResponse(true);
        }
        catch (System.Exception ex)
        {
            return exceptionHandler.HandleException(ex);
        }
    }

    public async Task<DefaultResponse> GetAllCharactersAsync(Guid accountGuid, CancellationToken token = default)
    {
        try
        {
            List<CharacterModel> characterModelList = mapper.Map<List<CharacterModel>>(
                await repository.GetAllCharactersAsync(accountGuid, token).ConfigureAwait(true));

            return new DefaultResponse(characterModelList);
        }
        catch (System.Exception ex)
        {
            return exceptionHandler.HandleException(ex);
        }
    }

    public async Task<DefaultResponse> GetCharacterByGuidAsync(Guid guid, CancellationToken token = default)
    {
        try
        {
            Character? charEntity = await repository.GetCharacterByIdAsync(guid, token).ConfigureAwait(true);

            if (charEntity is not null)
            {
                PlayerPosition? positionEntity = await positionRepository.GetPosition(charEntity.Id, token)
                    .ConfigureAwait(true);
                if (positionEntity is not null)
                {
                    charEntity.PlayerPosition = positionEntity;
                }
            }

            CharacterModel character = mapper.Map<CharacterModel>(charEntity);

            return new DefaultResponse(character);
        }
        catch (System.Exception ex)
        {
            return exceptionHandler.HandleException(ex);
        }
    }

    public async Task<DefaultResponse> UpdateCharacterAsync(CharacterModel model, CancellationToken token = default)
    {
        try
        {
            Character entity = mapper.Map<Character>(model);
            await repository.UpdateCharacterAsync(entity, token).ConfigureAwait(true);

            return new DefaultResponse(entity);
        }
        catch (System.Exception ex)
        {
            return exceptionHandler.HandleException(ex);
        }
    }
}