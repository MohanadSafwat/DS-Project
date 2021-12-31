using MarketPlace.Areas.Identity.Data;
using JWTAuthentication.Authentication;
using System.ComponentModel.DataAnnotations;

namespace MarketPlace.Models
{
    public class AssociatedBought
    {
        public int id { get; set; }
        public Product product { get; set; }

        public User Buyer { get; set; }
    }
}
