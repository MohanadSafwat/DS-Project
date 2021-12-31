using MarketPlace.Models;

namespace MarketPlace.Dtos
{
    public class ProductSharedReadDto
    {
        public Product product { get; set; }

        public string sharedId { get; set; }
        public string sharedFirstName { get; set; }
        public string sharedLastName { get; set; }
        public string sharedEmail { get; set; }
        public bool Sold { get; set; }
    }
}