using MarketPlace.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPlace.Models
{
    public class OrderItem
    {
        public User seller { get; set; }
        public Product Product { get; set; }
        public int Id { get; set; }
    }
}
