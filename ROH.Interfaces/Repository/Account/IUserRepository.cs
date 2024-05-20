namespace ROH.Interfaces.Repository.Account;

public interface IUserRepository
{
    Task<bool> EmailInUse(string email);
}