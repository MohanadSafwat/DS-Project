using JWTAuthentication.Authentication;

namespace MarketPlace.Models
{
    public class AssociatedSellSouth
    {
        public int id { get; set; }
        public Product productId { get; set; }

        public User2 SellerId { get; set; }

        public bool Sold { get; set; }
    }
}
