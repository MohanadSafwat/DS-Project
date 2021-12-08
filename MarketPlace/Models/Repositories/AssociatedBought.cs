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
        public List<AssociatedBought> FindProducts(string buyerId)
        {
            var result = db.AssociatedBought.Include(p => p.product).Include(s => s.Buyer).Where(s => s.Buyer.Id == buyerId).ToList();
            if (result != null)
                return result;
            else {
                List < AssociatedBought > list = new List<AssociatedBought>();
                list.Clear();
                return list;

            }

            /*            return db.AssociatedBought.Include(p => p.product).Include(s => s.Buyer).Where(s => s.Buyer.Id == buyerId).ToList();
            */
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
        public void Delete(int ProductId)
        {
            var AssociatedBought = Find(ProductId);
            db.AssociatedBought.Remove(AssociatedBought);
            db.SaveChanges();
        }

        public void Edit(AssociatedBought entity)
        {

            db.Update(entity);
            db.SaveChanges();
        }

        public AssociatedBought Find(int ProductId)
        {
            return db.AssociatedBought.Include(p => p.product).Include(s => s.Buyer).SingleOrDefault(p => p.product.ProductId == ProductId );
        }

        public IList<AssociatedBought> List()
        {
            return db.AssociatedBought.Include(s => s.Buyer).Include(p => p.product).ToList();
        }

    }
}
