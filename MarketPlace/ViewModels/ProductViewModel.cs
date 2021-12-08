using MarketPlace.Areas.Identity.Data;
using MarketPlace.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace MarketPlace.ViewModels
{
    public class ProductViewModel
    {
        public int ProductId { get; set; }

        public string SellerId { get; set; }


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

        public List<AssociatedSell> associatedSell { get; set; }
        public List<AssociatedShared> associatedShared { get; set; }
        public List<AssociatedSell> productsIndex { get; set; }
        public List<AssociatedSell> SearchedItems { get; set; }
        public List<AssociatedBought> associatedBought { get; set; }
        public AssociatedSell productDeatails { get; set; }

        public string searchTerm { get; set; }

    }
}
