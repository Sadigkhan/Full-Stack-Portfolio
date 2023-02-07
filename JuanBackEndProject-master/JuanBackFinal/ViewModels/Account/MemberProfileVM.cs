using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JuanBackFinal.ViewModels.Account
{
    public class MemberProfileVM
    {
        public MemberUpdateVM Member { get; set; }
        public List<JuanBackFinal.Models.Order> Orders { get; set; }
    }
}
