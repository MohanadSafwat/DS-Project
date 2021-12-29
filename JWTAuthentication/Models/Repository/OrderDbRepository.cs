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

        public OrderDbRepository(ApplicationDbContext _db)
        {
            db = _db;
        }

        public void Add(Order entity)
        {
            db.Orders.Add(entity);
            db.SaveChanges();
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
