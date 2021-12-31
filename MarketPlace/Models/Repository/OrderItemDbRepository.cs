using JWTAuthentication.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarketPlace.Areas.Identity.Data;

namespace MarketPlace.Models.Repository
{
    public class OrderItemDbRepository : IOrderRepository<OrderItem>
    {
        AppDBContext db;
        AppDB2Context db2;

        public OrderItemDbRepository(AppDBContext _db,AppDB2Context _db2)
        {
            db = _db;
            db2 = _db2;
        }

        public void Add(OrderItem entity)
        {
            db.OrderItems.Add(entity);
            db.SaveChanges();
        }
        public void AddOrderItemSouth(OrderItemSouth entity)
        {
            db2.OrderItemsSouth.Add(entity);
            db2.SaveChanges();
        }

        public void AddOrderSouth(OrderSouth entity)
        {
            throw new NotImplementedException();
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