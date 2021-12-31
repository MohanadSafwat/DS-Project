using JWTAuthentication.Authentication;
using MarketPlace.Areas.Identity.Data;

namespace MarketPlace.Models
{
    public class AssociatedShared
    {
        public int id { get; set; }
        public Product productId { get; set; }
        public User SharedId { get; set; }

        public bool Sold { get; set; }
    }
}
