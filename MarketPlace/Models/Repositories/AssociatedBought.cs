using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace MarketPlace.Models.Repositories
{
    public class AssociatedBoughtRepository : IAssociatedRepository<AssociatedBought>
    {
        AppDBContext db;

        public AssociatedBoughtRepository(AppDBContext _db)
        {
            db = _db;
        }
        public void Add(AssociatedBought entity)
        {
            db.AssociatedBought.Add(entity);
            db.SaveChanges();
        }
        public int IsExist(AssociatedBought entity)
        {
            return 0;
        }
        public void Delete(int ProductId, string SellerId)
        {
            var AssociatedBought = Find(ProductId, SellerId);
            db.AssociatedBought.Remove(AssociatedBought);
            db.SaveChanges();
        }

        public void Edit(AssociatedBought entity)
        {

            db.Update(entity);
            db.SaveChanges();
        }

        public AssociatedBought Find(int ProductId, string SellerId)
        {
            return db.AssociatedBought.SingleOrDefault(p => p.product.ProductId == ProductId && p.Buyer.Id == SellerId);
        }

        public IList<AssociatedBought> List()
        {
            return db.AssociatedBought.Include(s => s.Buyer).Include(p => p.product).ToList();
        }

    }
}
