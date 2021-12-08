/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPlace.Models.Repository
{
    public class OrderItemRepository
    {
        List<OrderItem> OrderItems;

        public OrderItemRepository()
        {
            OrderItems = new List<OrderItem>()
            {
                new OrderItem
                {
                    Id=1,
                    sellerId=2,
                    ProductId=3
                },
                new OrderItem*//**//*
                    Id=2,
                    sellerId=2,
                    ProductId=4
                },
                new OrderItem
                {
                    Id=3,
                    sellerId=3,
                    ProductId=3
                },
            };
        }

        public void Add(OrderItem entity)
        {
            entity.Id = OrderItems.Max(b => b.Id) + 1;
            OrderItems.Add(entity);
        }

        public void Delete(int id)
        {
            var orderItem = Find(id);

            OrderItems.Remove(orderItem);
        }

        public OrderItem Find(int id)
        {
            var orderItem = OrderItems.SingleOrDefault(b => b.Id == id);

            return orderItem;
        }

        public IList<OrderItem> List()
        {
            return OrderItems;
        }
        public List<OrderItem> SoldSellerItems(int SellerId)
        {
            return OrderItems.Where(o => o.sellerId== SellerId).ToList();
        }
    }
}
*/