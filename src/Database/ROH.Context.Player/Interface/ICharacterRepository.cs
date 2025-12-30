namespace ROH.Context.Player.Interface;

public interface ICharacterRepository
{
    Task AddCharacterAsync(Entities.Characters.Character character, CancellationToken cancellationToken = default);

    Task DeleteCharacterAsync(Guid id, CancellationToken cancellationToken = default);

    Task<List<Entities.Characters.Character>> GetAllCharactersAsync(Guid accountGuid, CancellationToken cancellationToken = default);

    Task<Entities.Characters.Character?> GetCharacterByIdAsync(Guid guid, CancellationToken cancellationToken = default);

    Task UpdateCharacterAsync(Entities.Characters.Character character, CancellationToken cancellationToken = default);
}