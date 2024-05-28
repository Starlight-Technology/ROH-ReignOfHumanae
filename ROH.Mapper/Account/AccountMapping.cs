using AutoMapper;

using ROH.StandardModels.Account;

namespace ROH.Mapping.Account;
public class AccountMapping : Profile
{
    public AccountMapping() => CreateMap<Domain.Accounts.Account, AccountModel>().ReverseMap();
}
