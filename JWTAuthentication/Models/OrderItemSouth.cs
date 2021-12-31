using JWTAuthentication.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPlace.Models
{
    public class OrderItemSouth
    {
        public User2 seller { get; set; }
        public Product Product { get; set; }
        public int Id { get; set; }
    }
}
