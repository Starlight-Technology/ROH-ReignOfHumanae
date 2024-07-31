using System;

namespace ROH.StandardModels.Account
{
    public class AccountModel
    {
        public Guid Guid { get; set; }
        public string? RealName { get; set; }
        public DateTime BirthDate { get; set; }
        public UserModel? User { get; set; }
    }
}