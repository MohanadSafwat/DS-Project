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
            var resultUnSold = db.AssociatedSharedSouthUnSold.Include(p => p.productId).Include(s => s.SharedId).Where(s => s.SharedId.Id == sellerId).ToList();
            var resultSold = db.AssociatedSharedSouthSold.Include(p => p.productId).Include(s => s.SharedId).Where(s => s.SharedId.Id == sellerId).ToList();

            if (resultUnSold == null)
            {
                return resultSold;
            }
            if (resultSold == null)
            {
                return resultUnSold;
            }

            return resultUnSold.Concat(resultSold).ToList();

        }
        public List<AssociatedSharedSouth> FindUsers(int productId)
        {
            var resultUnSold = db.AssociatedSharedSouthUnSold.Include(p => p.productId).Include(s => s.SharedId).Where(p => p.productId.ProductId == productId).ToList();
            var resultSold = db.AssociatedSharedSouthSold.Include(p => p.productId).Include(s => s.SharedId).Where(p => p.productId.ProductId == productId).ToList();

            if (resultUnSold == null)
            {
                return resultSold;
            }
            if (resultSold == null)
            {
                return resultUnSold;
            }

            return resultUnSold.Concat(resultSold).ToList();

        }
        public void Add(AssociatedSharedSouth entity)
        {
            db.AssociatedSharedSouthUnSold.Add(entity);
            db.SaveChanges();
        }
        public int IsExist(AssociatedSharedSouth entity)
        {
            return 0;
        }
        public void Delete(int ProductId)
        {
            var AssociatedSharedSouth = Find(ProductId);
            db.AssociatedSharedSouthUnSold.Remove(AssociatedSharedSouth);
            db.AssociatedSharedSouthSold.Remove(AssociatedSharedSouth);
            db.SaveChanges();
        }

        public void Edit(AssociatedSharedSouth entity)
        {

            db.Update(entity);
            db.SaveChanges();
        }
        public void EditList(List<AssociatedSharedSouth> entityList)
        {

            foreach (var entity in entityList)
            {
                db.Update(entity);
                db.SaveChanges();
            }

        }
        public List<AssociatedSharedSouth> Search(string term)
        {
            var result = db.AssociatedSharedSouthSold.Include(p => p.productId).Include(s => s.SharedId).Where(p => p.productId.ProductName.Contains(term)
               || p.productId.ProductBrand.Contains(term) || p.productId.ProductDescription.Contains(term) || p.SharedId.FirstName.Contains(term)
                   || p.SharedId.LastName.Contains(term)).ToList();
            return result;
        }
        public AssociatedSharedSouth Find(int ProductId)
        {
            var product = db.AssociatedSharedSouthUnSold.Include(p => p.productId).Include(s => s.SharedId).SingleOrDefault(p => p.productId.ProductId == ProductId);
            if (product != null)
                return product;
            else
            {
                var product2 = db.AssociatedSharedSouthSold.Include(p => p.productId).Include(s => s.SharedId).SingleOrDefault(p => p.productId.ProductId == ProductId);
                return product2;

            }
        }

        public List<AssociatedSharedSouth> List()
        {
            var resultUnSold = db.AssociatedSharedSouthUnSold.Include(s => s.SharedId).Include(p => p.productId).ToList();
            var resultSold = db.AssociatedSharedSouthSold.Include(s => s.SharedId).Include(p => p.productId).ToList();
            if (resultUnSold == null)
            {
                return resultSold;
            }
            if (resultSold == null)
            {
                return resultUnSold;
            }

            return resultUnSold.Concat(resultSold).ToList();
        }
        public List<ProductSharedReadDto> FindProductsDtos(string sharedId)
        {
   var resultUnSold = db.AssociatedSharedSouthUnSold.Select(x => new ProductSharedReadDto
            {
                sharedId = x.SharedId.Id,
                product = x.productId,
                sharedFirstName = x.SharedId.FirstName,
                sharedLastName = x.SharedId.LastName,
                sharedEmail = x.SharedId.Email
            }).Where(s => s.sharedId == sharedId).ToList();
            var resultSold = db.AssociatedSharedSouthSold.Select(x => new ProductSharedReadDto
            {
                sharedId = x.SharedId.Id,
                product = x.productId,
                sharedFirstName = x.SharedId.FirstName,
                sharedLastName = x.SharedId.LastName,
                sharedEmail = x.SharedId.Email
            }).Where(s => s.sharedId == sharedId).ToList();

            if (resultUnSold == null)
            {
                return resultSold;
            }
            if (resultSold == null)
            {
                return resultUnSold;
            }

            return resultUnSold.Concat(resultSold).ToList();
           
        }
        public List<ProductSharedReadDto> FindUnSoldProductsDtos(string sharedId)
        {

            var result = db.AssociatedSharedSouthUnSold.Select(x => new ProductSharedReadDto
            {
                sharedId = x.SharedId.Id,
                product = x.productId,
                sharedFirstName = x.SharedId.FirstName,
                sharedLastName = x.SharedId.LastName,
                sharedEmail = x.SharedId.Email
            }).Where(s => (s.sharedId == sharedId) ).ToList();
            if (result != null)
                return result;
            else
                return new List<ProductSharedReadDto>();
        }
        public List<ProductSharedReadDto> FindSoldProductsDtos(string sharedId)
        {

            var result = db.AssociatedSharedSouthSold.Select(x => new ProductSharedReadDto
            {
                sharedId = x.SharedId.Id,
                product = x.productId,
                sharedFirstName = x.SharedId.FirstName,
                sharedLastName = x.SharedId.LastName,
                sharedEmail = x.SharedId.Email
            }).Where(s => (s.sharedId == sharedId)).ToList();
            if (result != null)
                return result;
            else
                return new List<ProductSharedReadDto>();
        }
        public List<ProductSharedReadDto> FindUsersDtos(int productId)
        {
 var resultUnSold = db.AssociatedSharedSouthUnSold.Select(x => new ProductSharedReadDto
            {
                sharedId = x.SharedId.Id,
                product = x.productId,
                sharedFirstName = x.SharedId.FirstName,
                sharedLastName = x.SharedId.LastName,
                sharedEmail = x.SharedId.Email
            }).Where(p => p.product.ProductId == productId).ToList();
            var resultSold = db.AssociatedSharedSouthSold.Select(x => new ProductSharedReadDto
            {
                sharedId = x.SharedId.Id,
                product = x.productId,
                sharedFirstName = x.SharedId.FirstName,
                sharedLastName = x.SharedId.LastName,
                sharedEmail = x.SharedId.Email
            }).Where(p => p.product.ProductId == productId).ToList();
            if (resultUnSold == null)
            {
                return resultSold;
            }
            if (resultSold == null)
            {
                return resultUnSold;
            }

            return resultUnSold.Concat(resultSold).ToList();
           
        }



        public List<ProductSharedReadDto> SearchDtos(string term)
        {
            var result = db.AssociatedSharedSouthUnSold.Select(x => new ProductSharedReadDto
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
             var product = db.AssociatedSharedSouthUnSold.Select(x => new ProductSharedReadDto
            {
                sharedId = x.SharedId.Id,
                product = x.productId,
                sharedFirstName = x.SharedId.FirstName,
                sharedLastName = x.SharedId.LastName,
                sharedEmail = x.SharedId.Email
            }).SingleOrDefault(p => p.product.ProductId == ProductId);
            if (product != null)
                return product;
            else
            {
                var product2 = db.AssociatedSharedSouthSold.Select(x => new ProductSharedReadDto
            {
                sharedId = x.SharedId.Id,
                product = x.productId,
                sharedFirstName = x.SharedId.FirstName,
                sharedLastName = x.SharedId.LastName,
                sharedEmail = x.SharedId.Email
            }).SingleOrDefault(p => p.product.ProductId == ProductId);
                return product2;

            }
            
        }

        public List<ProductSharedReadDto> ListDtos()
        {
              var resultUnSold = db.AssociatedSharedSouthUnSold.Select(x => new ProductSharedReadDto
            {
                sharedId = x.SharedId.Id,
                product = x.productId,
                sharedFirstName = x.SharedId.FirstName,
                sharedLastName = x.SharedId.LastName,
                sharedEmail = x.SharedId.Email
            }).ToList();
            var resultSold = db.AssociatedSharedSouthSold.Select(x => new ProductSharedReadDto
            {
                sharedId = x.SharedId.Id,
                product = x.productId,
                sharedFirstName = x.SharedId.FirstName,
                sharedLastName = x.SharedId.LastName,
                sharedEmail = x.SharedId.Email
            }).ToList();

            if (resultUnSold == null)
            {
                return resultSold;
            }
            if (resultSold == null)
            {
                return resultUnSold;
            }

            return resultUnSold.Concat(resultSold).ToList();

          
        }

        public bool IsUserBuyThis(string accountId, int productId)
        {
            throw new System.NotImplementedException();
        }

        public bool IsUserShareThis(string accountId, int productId)
        {
            if ((db.AssociatedSharedSouthUnSold.Where(p => p.productId.ProductId == productId).Where(s => s.SharedId.Id == accountId) != null) ||( db.AssociatedSharedSouthSold.Where(p => p.productId.ProductId == productId).Where(s => s.SharedId.Id == accountId))!= null)
                return true;
            else
                return false;
        }
    }
}