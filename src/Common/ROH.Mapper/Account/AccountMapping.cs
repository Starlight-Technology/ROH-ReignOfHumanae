//-----------------------------------------------------------------------
// <copyright file="AccountMapping.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using AutoMapper;

using ROH.StandardModels.Account;

namespace ROH.Mapping.Account;

public class AccountMapping : Profile
{
    public AccountMapping()
    {
        _ = CreateMap<Context.Account.Entity.Account, AccountModel>()
            .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.BirthDate.ToDateTime(new TimeOnly(0, 0))));
        _ = CreateMap<AccountModel, Context.Account.Entity.Account>()
            .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.BirthDate)));
    }
}
