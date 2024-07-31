namespace ROH.Domain.Characters;

public record CharacterSkill(long Id, long IdCharacter, long IdSkill)
{
    public virtual Character? Character { get; set; }
    public virtual Skill? Skill { get; set; }
}