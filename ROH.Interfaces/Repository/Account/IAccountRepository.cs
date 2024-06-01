namespace ROH.Interfaces.Repository.Account;

public interface IAccountRepository
{
    Task<Domain.Accounts.Account?> GetAccountById(long id);

    Task<Domain.Accounts.Account?> GetAccountByGuid(Guid guid);

    Task<Domain.Accounts.Account?> GetAccountByUserGuid(Guid guid);

    Task UpdateAccount(Domain.Accounts.Account account);
}