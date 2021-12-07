using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace MarketPlace.ViewModels
{
    public class ProductViewModel
    {
        public int ProductId { get; set; }

        public int SellerId { get; set; }


        [Required]
        public string ProductName { get; set; }


        [Required]
        public string ProductDescription { get; set; }


       
        public string ProductImagesUrl { get; set; }

        [Required]
        public string ProductBrand { get; set; }

        [Required]
        public int ProductPrice { get; set; }

        public bool Sold { get; set; }


        [Required]
        public List<IFormFile> Files { get; set; }
    }
}
