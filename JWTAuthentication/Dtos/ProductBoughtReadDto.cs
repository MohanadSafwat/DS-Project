using MarketPlace.Models;

namespace MarketPlace.Dtos
{
    public class ProductBoughtReadDto
    {
        public Product product { get; set; }

        public string BuyerId { get; set; }
        public string BuyerFirstName { get; set; }
        public string BuyerLastName { get; set; }
        public string BuyerEmail { get; set; }
        public bool Sold { get; set; }
    }
}