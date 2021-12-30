using JWTAuthentication.Authentication;
using MarketPlace.Dtos;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace MarketPlace.Models.Repositories
{
    public class AssociatedSharedRepository : IAssociatedRepository<AssociatedShared>
    {
        ApplicationDbContext db;

        public AssociatedSharedRepository(ApplicationDbContext _db)
        {
            db = _db;  
        }
        public List<AssociatedShared> FindProducts(string sellerId)
        {
            
           var result= db.AssociatedShared.Include(p => p.productId).Include(s => s.SharedId).Where(s => s.SharedId.Id == sellerId).ToList();
            if (result != null)
                return result;
            else
                return new List<AssociatedShared>();
        }
        public List<AssociatedShared> FindUsers(int productId)
        {

            var result = db.AssociatedShared.Include(p => p.productId).Include(s => s.SharedId).Where(p => p.productId.ProductId == productId).ToList();
            if (result != null)
                return result;
            else
                return new List<AssociatedShared>();
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
        public void Delete(int ProductId)
        {
            var associatedShared = Find(ProductId);
            db.AssociatedShared.Remove(associatedShared);
            db.SaveChanges();
        }

        public void Edit(AssociatedShared entity)
        {
            
            db.Update(entity);
            db.SaveChanges();
        }
        public void EditList(List<AssociatedShared> entityList)
        {

            foreach (var entity in entityList) {
                db.Update(entity);
                db.SaveChanges();
            }
            
        }
        public List<AssociatedShared> Search(string term)
        {
            var result = db.AssociatedShared.Include(p => p.productId).Include(s => s.SharedId).Where(p => p.productId.ProductName.Contains(term)
               || p.productId.ProductBrand.Contains(term) || p.productId.ProductDescription.Contains(term) || p.SharedId.FirstName.Contains(term)
                   || p.SharedId.LastName.Contains(term)).ToList();
            return result;
        }
        public AssociatedShared Find(int ProductId)
        {
            return db.AssociatedShared.Include(p => p.productId).Include(s => s.SharedId).SingleOrDefault(p => p.productId.ProductId == ProductId);
        }

        public List<AssociatedShared> List()
        {
            return db.AssociatedShared.Include(s=>s.SharedId).Include(p=>p.productId).ToList();
        }

        List<ProductReadDto> IAssociatedRepository<AssociatedShared>.List()
        {
            throw new System.NotImplementedException();
        }

        List<ProductReadDto> IAssociatedRepository<AssociatedShared>.FindProducts(string sellerId)
        {
            throw new System.NotImplementedException();
        }
    }
}
