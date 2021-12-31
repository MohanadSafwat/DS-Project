using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPlace.Models.Repository
{
     public interface IOrderRepository<TEntity>
    {
        public void Add(TEntity entity);
        public void AddOrderSouth(OrderSouth entity);
        public void AddOrderItemSouth(OrderItemSouth entity);
        public void Delete(int id);

        public TEntity Find(int id);

        public IList<TEntity> List();
    }
}
