using ROH.Domain.Characters;

namespace ROH.Domain.Accounts
{
    public record Account(long Id, long IdUser, Guid Guid, string? UserName, string? RealName, DateOnly BirthDate)
    {
        public ICollection<Character>? Characters { get; init; }
        public User? User { get; init; }
    }
}