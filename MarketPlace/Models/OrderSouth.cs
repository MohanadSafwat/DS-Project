using JWTAuthentication.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPlace.Models
{
    public class OrderSouth
    {
        public int Id { get; set; }
        public User2 Customer { get; set; }
        public OrderItemSouth OrderItemSouth { get; set; }
    }
}
