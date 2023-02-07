using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DekorEvStartUpFinal.Models
{
    public  class AppUser:IdentityUser
    {
        [StringLength(255)]
        public string FullName { get; set; }
        public bool isAdmin { get; set; }
        public bool isMarket { get; set; }
        [StringLength(500)]
        public string Adress { get; set; }
        [StringLength(50)]
        public string City { get; set; }
        [StringLength(1000)]
        public string ProfileImage { get; set; }
        [NotMapped]
        public IFormFile ProfileImageFile { get; set; }
        public string EmailConfirmationToken { get; set; }
        public string PasswordResetToken { get; set; }
        [StringLength(5000)]
        public string Description { get; set; }
        [StringLength(1000)]
        public string UserImage { get; set; }
        [NotMapped]
        public IFormFile UserImageFile { get; set; }

        public IEnumerable<Basket> Baskets { get; set; }
        public IEnumerable<Compare> Compares { get; set; }
        public IEnumerable<Product> Products { get; set; }
        public ViewCount ViewCount { get; set; }
        public bool isConfirmed { get; set; }
        public string ConnectionId { get; set; }



    }
}
