using AutoMapper;

using ROH.Domain.Accounts;
using ROH.StandardModels.Account;

namespace ROH.Mapper.Version;
public class UserMapping : Profile
{
    public UserMapping() => CreateMap<User, UserModel>().ReverseMap();
}
