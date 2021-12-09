namespace RDLCDesign
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Product
    {
        public int ProductId { get; set; }

        public int ProductPrice { get; set; }

        public string ProductBrand { get; set; }

        public string ProductDescription { get; set; }

        public string ProductName { get; set; }

        public string ProductImageUrls { get; set; }
    }
}
