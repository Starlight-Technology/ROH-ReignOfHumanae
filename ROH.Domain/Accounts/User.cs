namespace ROH.Domain.Accounts
{
    public record User(long Id, long IdAccount, Guid Guid, string? Email, string? Password)
    {
        public virtual Account? Account { get; set; }
    }
}