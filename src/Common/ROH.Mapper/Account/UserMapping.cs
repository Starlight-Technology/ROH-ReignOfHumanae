//-----------------------------------------------------------------------
// <copyright file="UserMapping.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using AutoMapper;

using ROH.Context.Account.Entity;
using ROH.StandardModels.Account;

namespace ROH.Mapping.Account;

public class UserMapping : Profile
{
    public UserMapping() => CreateMap<User, UserModel>().ReverseMap();
}