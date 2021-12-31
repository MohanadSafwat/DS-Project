using JWTAuthentication.Authentication;
using MarketPlace.Dtos;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace MarketPlace.Models.Repositories
{
    public class SouthAssociatedSharedRepository : IAssociatedRepository<AssociatedSharedSouth, ProductSharedReadDto>
    {
        ApplicationDb2Context db;

        public SouthAssociatedSharedRepository(ApplicationDb2Context _db)
        {
            db = _db;
        }

        public List<AssociatedSharedSouth> FindProducts(string sellerId)
        {
            
           var result= db.AssociatedSharedSouth.Include(p => p.productId).Include(s => s.SharedId).Where(s => s.SharedId.Id == sellerId).ToList();
            if (result != null)
                return result;
            else
                return new List<AssociatedSharedSouth>();
        }
        public List<AssociatedSharedSouth> FindUsers(int productId)
        {

            var result = db.AssociatedSharedSouth.Include(p => p.productId).Include(s => s.SharedId).Where(p => p.productId.ProductId == productId).ToList();
            if (result != null)
                return result;
            else
                return new List<AssociatedSharedSouth>();
        }
        public void Add(AssociatedSharedSouth entity)
        {
            db.AssociatedSharedSouth.Add(entity);
            db.SaveChanges();
        }
        public int IsExist(AssociatedSharedSouth entity)
        {
            return 0;
        }
        public void Delete(int ProductId)
        {
            var AssociatedSharedSouth = Find(ProductId);
            db.AssociatedSharedSouth.Remove(AssociatedSharedSouth);
            db.SaveChanges();
        }

        public void Edit(AssociatedSharedSouth entity)
        {
            
            db.Update(entity);
            db.SaveChanges();
        }
        public void EditList(List<AssociatedSharedSouth> entityList)
        {

            foreach (var entity in entityList) {
                db.Update(entity);
                db.SaveChanges();
            }
            
        }
        public List<AssociatedSharedSouth> Search(string term)
        {
            var result = db.AssociatedSharedSouth.Include(p => p.productId).Include(s => s.SharedId).Where(p => p.productId.ProductName.Contains(term)
               || p.productId.ProductBrand.Contains(term) || p.productId.ProductDescription.Contains(term) || p.SharedId.FirstName.Contains(term)
                   || p.SharedId.LastName.Contains(term)).ToList();
            return result;
        }
        public AssociatedSharedSouth Find(int ProductId)
        {
            return db.AssociatedSharedSouth.Include(p => p.productId).Include(s => s.SharedId).SingleOrDefault(p => p.productId.ProductId == ProductId);
        }

        public List<AssociatedSharedSouth> List()
        {
            return db.AssociatedSharedSouth.Include(s=>s.SharedId).Include(p=>p.productId).ToList();
        }
        public List<ProductSharedReadDto> FindProductsDtos(string sharedId)
        {

            var result = db.AssociatedSharedSouth.Select(x => new ProductSharedReadDto
            {
                sharedId = x.SharedId.Id,
                product = x.productId,
                sharedFirstName = x.SharedId.FirstName,
                sharedLastName = x.SharedId.LastName,
                sharedEmail = x.SharedId.Email
            }).Where(s => s.sharedId == sharedId).ToList();
            if (result != null)
                return result;
            else
                return new List<ProductSharedReadDto>();
        }
        public List<ProductSharedReadDto> FindUnSoldProductsDtos(string sharedId)
        {

            var result = db.AssociatedSharedSouth.Select(x => new ProductSharedReadDto
            {
                sharedId = x.SharedId.Id,
                product = x.productId,
                sharedFirstName = x.SharedId.FirstName,
                sharedLastName = x.SharedId.LastName,
                sharedEmail = x.SharedId.Email
            }).Where(s => (s.sharedId == sharedId )&& (!s.Sold)).ToList();
            if (result != null)
                return result;
            else
                return new List<ProductSharedReadDto>();
        }
           public List<ProductSharedReadDto> FindSoldProductsDtos(string sharedId)
        {

            var result = db.AssociatedSharedSouth.Select(x => new ProductSharedReadDto
            {
                sharedId = x.SharedId.Id,
                product = x.productId,
                sharedFirstName = x.SharedId.FirstName,
                sharedLastName = x.SharedId.LastName,
                sharedEmail = x.SharedId.Email
            }).Where(s => (s.sharedId == sharedId )&& (s.Sold)).ToList();
            if (result != null)
                return result;
            else
                return new List<ProductSharedReadDto>();
        }
        public List<ProductSharedReadDto> FindUsersDtos(int productId)
        {

            var result = db.AssociatedSharedSouth.Select(x => new ProductSharedReadDto
            {
                sharedId = x.SharedId.Id,
                product = x.productId,
                sharedFirstName = x.SharedId.FirstName,
                sharedLastName = x.SharedId.LastName,
                sharedEmail = x.SharedId.Email
            }).Where(p => p.product.ProductId == productId).ToList();
            if (result != null)
                return result;
            else
                return new List<ProductSharedReadDto>();
        }
      

    
        public List<ProductSharedReadDto> SearchDtos(string term)
        {
            var result = db.AssociatedSharedSouth.Select(x => new ProductSharedReadDto
            {
                sharedId = x.SharedId.Id,
                product = x.productId,
                sharedFirstName = x.SharedId.FirstName,
                sharedLastName = x.SharedId.LastName,
                sharedEmail = x.SharedId.Email
            }).Where(p => p.product.ProductName.Contains(term)
               || p.product.ProductBrand.Contains(term) || p.product.ProductDescription.Contains(term) || p.sharedFirstName.Contains(term)
                   || p.sharedLastName.Contains(term)).ToList();
            return result;
        }
        public ProductSharedReadDto FindProductByIdDtos(int ProductId)
        {
            return db.AssociatedSharedSouth.Select(x => new ProductSharedReadDto
            {
                sharedId = x.SharedId.Id,
                product = x.productId,
                sharedFirstName = x.SharedId.FirstName,
                sharedLastName = x.SharedId.LastName,
                sharedEmail = x.SharedId.Email
            }).SingleOrDefault(p => p.product.ProductId == ProductId);
        }

        public List<ProductSharedReadDto> ListDtos()
        {
            return db.AssociatedSharedSouth.Select(x => new ProductSharedReadDto
            {
                sharedId = x.SharedId.Id,
                product = x.productId,
                sharedFirstName = x.SharedId.FirstName,
                sharedLastName = x.SharedId.LastName,
                sharedEmail = x.SharedId.Email
            }).ToList();
        }

        public bool IsUserBuyThis(string accountId, int productId)
        {
            throw new System.NotImplementedException();
        }

        public bool IsUserShareThis(string accountId, int productId)
        {
            if( db.AssociatedSharedSouth.Where(p => p.productId.ProductId == productId).Where(s=>s.SharedId.Id == accountId) != null)
            return true;
            else
            return false;
        }
    }
}