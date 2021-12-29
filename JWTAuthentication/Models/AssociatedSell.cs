using JWTAuthentication.Authentication;

namespace MarketPlace.Models
{
    public class AssociatedSell
    {
        public int id { get; set; }
        public Product productId { get; set; }

        public User SellerId { get; set; }

        public bool Sold { get; set; }
    }
}
