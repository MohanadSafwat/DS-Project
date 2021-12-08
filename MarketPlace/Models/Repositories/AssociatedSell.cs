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
        public void Delete(int ProductId, string SellerId)
        {
            var associatedSell = Find(ProductId, SellerId);
            db.AssociatedSell.Remove(associatedSell);
            db.SaveChanges();
        }

        public void Edit(AssociatedSell entity)
        {

            db.Update(entity);
            db.SaveChanges();
        }

        public AssociatedSell Find(int ProductId, string SellerId)
        {
            return db.AssociatedSell.SingleOrDefault(p => p.productId.ProductId == ProductId && p.SellerId.Id == SellerId);
        }

        public IList<AssociatedSell> List()
        {
            return db.AssociatedSell.Include(s => s.SellerId).Include(p => p.productId).ToList();
        }

    }
}
