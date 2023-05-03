namespace ROH.Domain.Accounts
{
    public record User(long Id, long IdAccount, string? Email, string? Password)
    {
        public virtual Account Account { get; set; }
    }
}
