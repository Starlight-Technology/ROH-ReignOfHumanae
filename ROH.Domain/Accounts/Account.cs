using ROH.Domain.Characters;

namespace ROH.Domain.Accounts
{
    public class Account
    {
        public long Id { get; set; }
        public long IdUser { get; set; }
        public string? UserName { get; set; }
        public string? RealName { get; set; }
        public DateOnly BirthDate { get; set; }
        public virtual ICollection<Character>? Characters { get; set; }
    }
}