//-----------------------------------------------------------------------
// <copyright file="CharacterRepository.cs" company="Starlight-Technology">
//     Author:  
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using Microsoft.EntityFrameworkCore;

using ROH.Context.Player.Interface;

namespace ROH.Context.Player.Repository;

public class CharacterRepository(IPlayerContext context) : ICharacterRepository
{
    public async Task AddCharacterAsync(
        Entities.Characters.Character character,
        CancellationToken cancellationToken = default)
    {
        character.DateCreated = DateTime.UtcNow;

        await context.Characters.AddAsync(character, cancellationToken).ConfigureAwait(true);
        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(true);
    }

    public async Task DeleteCharacterAsync(Guid id, CancellationToken cancellationToken = default)
    {
        Entities.Characters.Character? character = await GetCharacterByIdAsync(id, cancellationToken)
            .ConfigureAwait(true);
        if (character != null)
        {
            context.Characters.Remove(character);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(true);
        }
    }

    public async Task<List<Entities.Characters.Character>> GetAllCharactersAsync(
        Guid accountGuid,
        CancellationToken cancellationToken = default) => await context.Characters
        .Where(c => c.GuidAccount == accountGuid)
        .ToListAsync(cancellationToken)
        .ConfigureAwait(true);

    public async Task<Entities.Characters.Character?> GetCharacterByIdAsync(
        Guid guid,
        CancellationToken cancellationToken = default) => await context.Characters
        .FirstOrDefaultAsync(c => c.Guid == guid, cancellationToken)
        .ConfigureAwait(true);

    public async Task UpdateCharacterAsync(
        Entities.Characters.Character character,
        CancellationToken cancellationToken = default)
    {
        context.Characters.Update(character);
        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(true);
    }
}