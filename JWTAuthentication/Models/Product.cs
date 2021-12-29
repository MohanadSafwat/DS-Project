using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace MarketPlace.Models
{
    public class Product
    {
        [Required]

        public int ProductId { get; set; }

        public int ProductPrice { get; set; }

        public string ProductBrand { get; set; }

        public string ProductDescription { get; set; }


        public string ProductName { get; set; }

        
        public string ProductImageUrls { get; set; }




    }


}
