using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kontakt.ViewModels
{
    public class UserProfileVM
    {
        public UserUpdateVM Member { get; set; }
        public UserInfoUpdateVM MemberInfo { get; set; }
        public List<Models.Order> Orders { get; set; }
        public List<WishVM> WishVMs { get; set; }
        public List<BasketVM> BasketVMs { get; set; }
        public List<Models.Review> Reviews { get; set; }
        public List<LikeVM> LikeVMs { get; set; }
        public List<DisLikeVM> DisLikeVMs { get; set; }
    }
}
