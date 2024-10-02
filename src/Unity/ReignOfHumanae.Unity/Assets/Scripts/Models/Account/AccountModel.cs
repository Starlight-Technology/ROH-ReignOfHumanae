using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Models.Account
{
    [Serializable]
    public class AccountModel
    {
        public Guid Guid { get; set; }
        public string RealName { get; set; }
        public DateTime BirthDate { get; set; }
        public UserModel User { get; set; }
    }
}
