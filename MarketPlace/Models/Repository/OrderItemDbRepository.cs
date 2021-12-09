using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPlace.Models.Repository
{
    public class OrderItemDbRepository : IOrderRepository<OrderItem>
    {
        AppDBContext db;

        public OrderItemDbRepository(AppDBContext _db)
        {
            db = _db;
        }

        public void Add(OrderItem entity)
        {
            db.OrderItems.Add(entity);
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            var orderItem = Find(id);

            db.OrderItems.Remove(orderItem);
            db.SaveChanges();

        }

        public OrderItem Find(int id)
        {
            var orderItem = db.OrderItems.SingleOrDefault(b => b.Id == id);

            return orderItem;
        }

        public IList<OrderItem> List()
        {
            return db.OrderItems.ToList();
        }
        public List<OrderItem> SoldSellerItems(int SellerId)
        {
            // edit id to int 

            return db.OrderItems.Where(o => o.seller.Id == SellerId.ToString()).ToList();
        }
    }
}