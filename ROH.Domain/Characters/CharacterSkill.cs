namespace ROH.Domain.Characters
{
    public record CharacterSkill(long Id, long IdCharacter, long IdSkill, Character Character, Skill Skill)
    {
    }
}