using AutoMapper;

using ROH.Domain.Accounts;
using ROH.StandardModels.Account;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROH.Mapper.Version;
public class UserMapping : Profile
{
    public UserMapping() => CreateMap<User, UserModel>().ReverseMap();
}
