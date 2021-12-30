using JWTAuthentication.Authentication;
using MarketPlace.Dtos;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace MarketPlace.Models.Repositories
{
    public class AssociatedSharedRepository : IAssociatedRepository<AssociatedShared, ProductSharedReadDto>
    {
        ApplicationDbContext db;

        public AssociatedSharedRepository(ApplicationDbContext _db)
        {
            db = _db;
        }
        public List<ProductSharedReadDto> FindProducts(string sharedId)
        {

            var result = db.AssociatedShared.Select(x => new ProductSharedReadDto
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
        public List<ProductSharedReadDto> FindUsers(int productId)
        {

            var result = db.AssociatedShared.Select(x => new ProductSharedReadDto
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

            foreach (var entity in entityList)
            {
                db.Update(entity);
                db.SaveChanges();
            }

        }
        public List<ProductSharedReadDto> Search(string term)
        {
            var result = db.AssociatedShared.Select(x => new ProductSharedReadDto
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
        public ProductSharedReadDto FindProductById(int ProductId)
        {
            return db.AssociatedShared.Select(x => new ProductSharedReadDto
            {
                sharedId = x.SharedId.Id,
                product = x.productId,
                sharedFirstName = x.SharedId.FirstName,
                sharedLastName = x.SharedId.LastName,
                sharedEmail = x.SharedId.Email
            }).SingleOrDefault(p => p.product.ProductId == ProductId);
        }
        public AssociatedShared Find(int ProductId)
        {
            return db.AssociatedShared.Include(s => s.SharedId).Include(p => p.productId).SingleOrDefault(p => p.productId.ProductId == ProductId);
        }
        public List<ProductSharedReadDto> List()
        {
            return db.AssociatedShared.Select(x => new ProductSharedReadDto
            {
                sharedId = x.SharedId.Id,
                product = x.productId,
                sharedFirstName = x.SharedId.FirstName,
                sharedLastName = x.SharedId.LastName,
                sharedEmail = x.SharedId.Email
            }).ToList();
        }


    }
}
