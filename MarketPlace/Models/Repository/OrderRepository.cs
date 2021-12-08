/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPlace.Models.Repository
{
    public class OrderRepository
    {
        List<Order> Orders;

        public OrderRepository()
        {
            Orders = new List<Order>()
            {
                new Order
                {
                    Id=1,
                    CustomerId=2,
                    OrderItem=new OrderItem{Id=3,ProductId=3,sellerId= 2}
                },
                new Order
                {
                    Id=2,
                    CustomerId=2,
                    OrderItem=new OrderItem{Id=3,ProductId=4,sellerId= 2}
                },
                new Order
                {
                    Id=3,
                    CustomerId=2,
                    OrderItem=new OrderItem{Id=3,ProductId=3,sellerId= 3}
                },
            };
        }

        public void Add(Order entity)
        {
            entity.Id = Orders.Max(b => b.Id) + 1;
            Orders.Add(entity);
        }

        public void Delete(int id)
        {
            var order = Find(id);

            Orders.Remove(order);
        }

        public Order Find(int id)
        {
            var order = Orders.SingleOrDefault(b => b.Id == id);

            return order;
        }

        public IList<Order> List()
        {
            return Orders;
        }
    }
}
*/