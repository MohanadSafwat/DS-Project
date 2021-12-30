using MarketPlace.Models;

namespace MarketPlace.Dtos
{
    public class ProductSellerReadDto
    {
        public Product product { get; set; }

        public string sellerId { get; set; }
        public string sellerFirstName { get; set; }
        public string sellerLastName { get; set; }
        public string sellerEmail { get; set; }
        public bool Sold { get; set; }
    }
}