using AutoMapper;

using ROH.StandardModels.Account;

namespace ROH.Mapping.Account;
public class AccountMapping : Profile
{
    public AccountMapping()
    {
        CreateMap<Domain.Accounts.Account, AccountModel>()
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.BirthDate.ToDateTime(new TimeOnly(0, 0))));
        CreateMap<AccountModel, Domain.Accounts.Account>()
            .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.BirthDate)));
    }
}
