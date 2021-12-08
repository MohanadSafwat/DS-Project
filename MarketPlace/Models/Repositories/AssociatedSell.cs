using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace MarketPlace.Models.Repositories
{
    public class AssociatedSellRepository : IAssociatedRepository<AssociatedSell>
    {
        AppDBContext db;

        public AssociatedSellRepository(AppDBContext _db)
        {
            db = _db;
        }
        public void Add(AssociatedSell entity)
        {
            db.AssociatedSell.Add(entity);
            db.SaveChanges();
        }
        public int IsExist(AssociatedSell entity)
        {
            return 0;
        }
        public void Delete(int ProductId)
        {
            var associatedSell = Find(ProductId);
            db.AssociatedSell.Remove(associatedSell);
            db.SaveChanges();
        }

        public void Edit(AssociatedSell entity)
        {

            db.Update(entity);
            db.SaveChanges();
        }
        public void EditList(List<AssociatedSell> entityList)
        {

            db.Update(entityList);
            db.SaveChanges();
        }
        public List<AssociatedSell> FindUsers(int productId)
        {

            var result = db.AssociatedSell.Include(p => p.productId).Include(s => s.SellerId).Where(p => p.productId.ProductId == productId).ToList();
            if (result != null)
                return result;
            else
                return new List<AssociatedSell>();
        }
        public AssociatedSell Find(int ProductId)
        {
            return db.AssociatedSell.Include(p=>p.productId).Include(s=>s.SellerId).SingleOrDefault(p => p.productId.ProductId == ProductId );
        }

        public List<AssociatedSell> FindProducts(string sellerId)
        {
            return db.AssociatedSell.Include(p => p.productId).Include(s => s.SellerId).Where(s => s.SellerId.Id == sellerId).ToList();
        }

        public IList<AssociatedSell> List()
        {
            return db.AssociatedSell.Include(s => s.SellerId).Include(p => p.productId).ToList();
        }

    }
}
