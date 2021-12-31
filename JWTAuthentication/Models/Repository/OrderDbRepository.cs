using JWTAuthentication.Authentication;
using MarketPlace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPlace.Models.Repository
{
    public class OrderDbRepository:IOrderRepository<Order>
    {
        ApplicationDbContext db;
        ApplicationDb2Context db2;
        public OrderDbRepository(ApplicationDbContext _db,ApplicationDb2Context _db2)
        {
            db = _db;
            db2 = _db2;
        }

        public void Add(Order entity)
        {
            db.Orders.Add(entity);
            db.SaveChanges();
        }

        public void AddOrderItemSouth(OrderItemSouth entity)
        {
            throw new NotImplementedException();
        }

        public void AddOrderSouth(OrderSouth entity)
        {
            db2.OrdersSouth.Add(entity);
            db2.SaveChanges();
        }

        public void Delete(int id)
        {
            var order = Find(id);

            db.Orders.Remove(order);
            db.SaveChanges();

        }

        public Order Find(int id)
        {
            var order = db.Orders.SingleOrDefault(b => b.Id == id);

            return order;
        }

        public IList<Order> List()
        {
            return db.Orders.ToList();
        }
    }
}
