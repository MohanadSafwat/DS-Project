using MarketPlace.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPlace.Models
{
    public class Transaction
    {
        [Key]
        public int Id { get; set; }
        public string CustomerEmail { get; set; }
        public string SellerEmail { get; set; }
        public string ItemName { get; set; }
        public int itemPrice { get; set; }
    }
}
