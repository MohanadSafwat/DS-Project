using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPlace.Models.Repository
{
    public class OrderItemDbRepository
    {
        MarketPlaceDbContext db;

        public OrderItemDbRepository(MarketPlaceDbContext _db)
        {
            db = _db;
        }

        public void Add(OrderItem entity)
        {
            db.OrderItems.Add(entity);
        }

        public void Delete(int id)
        {
            var orderItem = Find(id);

            db.OrderItems.Remove(orderItem);
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
            return db.OrderItems.Where(o => o.sellerId == SellerId).ToList();
        }
    }
}