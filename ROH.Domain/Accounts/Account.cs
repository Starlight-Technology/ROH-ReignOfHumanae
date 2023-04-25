using ROH.Domain.Characters;

namespace ROH.Domain.Accounts
{
    public record Account(long Id, long IdUser, string? UserName, string? RealName, DateOnly BirthDate, ICollection<Character>? Characters);
}