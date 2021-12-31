using JWTAuthentication.Authentication;

namespace MarketPlace.Models
{
    public class AssociatedSharedSouth
    {
        public int id { get; set; }
        public Product productId { get; set; }
        public User2 SharedId { get; set; }

        public bool Sold { get; set; }
    }
}
