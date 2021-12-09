using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPlace.Models.Repository
{
    interface IOrderRepository<TEntity>
    {
        public void Add(TEntity entity);

        public void Delete(int id);

        public TEntity Find(int id);

        public IList<TEntity> List();
    }
}
