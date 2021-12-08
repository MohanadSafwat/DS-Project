using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace MarketPlace.Models.Repositories
{
    public class AssociatedSharedRepository : IAssociatedRepository<AssociatedShared>
    {
        AppDBContext db;

        public AssociatedSharedRepository(AppDBContext _db)
        {
            db = _db;  
        }
        public void Add(AssociatedShared entity)
        {
            db.AssociatedShared.Add(entity);
            db.SaveChanges();
        }
        public int IsExist(AssociatedShared entity)
        {
            return 0;
        }
        public void Delete(int ProductId, string SellerId)
        {
            var associatedShared = Find(ProductId,SellerId);
            db.AssociatedShared.Remove(associatedShared);
            db.SaveChanges();
        }

        public void Edit(AssociatedShared entity)
        {
            
            db.Update(entity);
            db.SaveChanges();
        }

        public AssociatedShared Find(int ProductId, string SellerId)
        {
            return db.AssociatedShared.SingleOrDefault(p => p.productId.ProductId == ProductId && p.SharedId.Id == SellerId);
        }

        public IList<AssociatedShared> List()
        {
            return db.AssociatedShared.Include(s=>s.SharedId).Include(p=>p.productId).ToList();
        }

    }
}
