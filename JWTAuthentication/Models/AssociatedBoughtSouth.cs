using JWTAuthentication.Authentication;
using System.ComponentModel.DataAnnotations;

namespace MarketPlace.Models
{
    public class AssociatedBoughtSouth
    {
        public int id { get; set; }
        public Product product { get; set; }

        public User2 Buyer { get; set; }
    }
}
