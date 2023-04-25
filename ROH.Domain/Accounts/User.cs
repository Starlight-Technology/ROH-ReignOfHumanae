namespace ROH.Domain.Accounts
{
    public class User
    {
        public long Id { get; set; }
        public long IdAccount { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }

        public virtual Account? Account { get; set; }
    }
}
