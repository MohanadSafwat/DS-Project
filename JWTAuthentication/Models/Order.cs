using JWTAuthentication.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPlace.Models
{
    public class Order
    {
        public int Id { get; set; }
        public User Customer { get; set; }
        public OrderItem OrderItem { get; set; }
    }
}
