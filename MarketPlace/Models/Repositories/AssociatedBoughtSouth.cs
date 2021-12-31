using JWTAuthentication.Authentication;
using MarketPlace.Dtos;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace MarketPlace.Models.Repositories
{
    public class SouthAssociatedBoughtRepository : IAssociatedRepository<AssociatedBoughtSouth, ProductBoughtReadDto>
    {
        AppDB2Context db;

        public SouthAssociatedBoughtRepository(AppDB2Context _db)
        {
            db = _db;
        }
        public List<AssociatedBoughtSouth> FindUsers(int productId)
        {

            var result = db.AssociatedBoughtSouth.Include(p => p.product).Include(s => s.Buyer).Where(p => p.product.ProductId == productId).ToList();
            if (result != null)
                return result;
            else
                return new List<AssociatedBoughtSouth>();
        }
        public void EditList(List<AssociatedBoughtSouth> entityList)
        {

            db.Update(entityList);
            db.SaveChanges();
        }
        public List<AssociatedBoughtSouth> FindProducts(string buyerId)
        {
            var result = db.AssociatedBoughtSouth.Include(p => p.product).Include(s => s.Buyer).Where(s => s.Buyer.Id == buyerId).ToList();
            if (result != null)
                return result;
            else
            {
                List<AssociatedBoughtSouth> list = new List<AssociatedBoughtSouth>();
                list.Clear();
                return list;

            }

            /*            return db.AssociatedBoughtSouth.Include(p => p.product).Include(s => s.Buyer).Where(s => s.Buyer.Id == buyerId).ToList();
            */
        }
        public List<AssociatedBoughtSouth> Search(string term)
        {
            var result = db.AssociatedBoughtSouth.Include(p => p.product).Include(s => s.Buyer).Where(p => p.product.ProductName.Contains(term)
               || p.product.ProductBrand.Contains(term) || p.product.ProductDescription.Contains(term) || p.Buyer.FirstName.Contains(term)
                   || p.Buyer.LastName.Contains(term)).ToList();
            return result;
        }
        public void Add(AssociatedBoughtSouth entity)
        {
            db.AssociatedBoughtSouth.Add(entity);
            db.SaveChanges();
        }
        public int IsExist(AssociatedBoughtSouth entity)
        {
            return 0;
        }
        public void Delete(int ProductId)
        {
            var AssociatedBoughtSouth = Find(ProductId);
            db.AssociatedBoughtSouth.Remove(AssociatedBoughtSouth);
            db.SaveChanges();
        }

        public void Edit(AssociatedBoughtSouth entity)
        {

            db.Update(entity);
            db.SaveChanges();
        }

        public AssociatedBoughtSouth Find(int ProductId)
        {
            return db.AssociatedBoughtSouth.Include(p => p.product).Include(s => s.Buyer).SingleOrDefault(p => p.product.ProductId == ProductId);
        }

        public List<AssociatedBoughtSouth> List()
        {
            return db.AssociatedBoughtSouth.Include(s => s.Buyer).Include(p => p.product).ToList();
        }
        public List<ProductBoughtReadDto> FindUsersDtos(int productId)
        {

            var result = db.AssociatedBoughtSouth.Select(x => new ProductBoughtReadDto
            {
                BuyerId = x.Buyer.Id,
                product = x.product,
                BuyerFirstName = x.Buyer.FirstName,
                BuyerLastName = x.Buyer.LastName,
                BuyerEmail = x.Buyer.Email
            }).Where(p => p.product.ProductId == productId).ToList();
            if (result != null)
                return result;
            else
                return new List<ProductBoughtReadDto>();
        }

        public List<ProductBoughtReadDto> FindProductsDtos(string buyerId)
        {
            var result = db.AssociatedBoughtSouth.Select(x => new ProductBoughtReadDto
            {
                BuyerId = x.Buyer.Id,
                product = x.product,
                BuyerFirstName = x.Buyer.FirstName,
                BuyerLastName = x.Buyer.LastName,
                BuyerEmail = x.Buyer.Email
            }).Where(b => b.BuyerId == buyerId).ToList();
            if (result != null)
                return result;
            else
            {
                List<ProductBoughtReadDto> list = new List<ProductBoughtReadDto>();
                list.Clear();
                return list;

            }

        }
        public List<ProductBoughtReadDto> FindSoldProductsDtos(string buyerId)
        {
            var result = db.AssociatedBoughtSouth.Select(x => new ProductBoughtReadDto
            {
                BuyerId = x.Buyer.Id,
                product = x.product,
                BuyerFirstName = x.Buyer.FirstName,
                BuyerLastName = x.Buyer.LastName,
                BuyerEmail = x.Buyer.Email
            }).Where(b => b.BuyerId == buyerId && b.Sold).ToList();
            if (result != null)
                return result;
            else
            {
                List<ProductBoughtReadDto> list = new List<ProductBoughtReadDto>();
                list.Clear();
                return list;

            }

        }
        public List<ProductBoughtReadDto> FindUnSoldProductsDtos(string buyerId)
        {
            var result = db.AssociatedBoughtSouth.Select(x => new ProductBoughtReadDto
            {
                BuyerId = x.Buyer.Id,
                product = x.product,
                BuyerFirstName = x.Buyer.FirstName,
                BuyerLastName = x.Buyer.LastName,
                BuyerEmail = x.Buyer.Email
            }).Where(b => b.BuyerId == buyerId && !b.Sold).ToList();
            if (result != null)
                return result;
            else
            {
                List<ProductBoughtReadDto> list = new List<ProductBoughtReadDto>();
                list.Clear();
                return list;

            }

        }
        public List<ProductBoughtReadDto> SearchDtos(string term)
        {
            var result = db.AssociatedBoughtSouth.Select(x => new ProductBoughtReadDto
            {
                BuyerId = x.Buyer.Id,
                product = x.product,
                BuyerFirstName = x.Buyer.FirstName,
                BuyerLastName = x.Buyer.LastName,
                BuyerEmail = x.Buyer.Email
            }).Where(p => p.product.ProductName.Contains(term)
               || p.product.ProductBrand.Contains(term) || p.product.ProductDescription.Contains(term) || p.BuyerFirstName.Contains(term)
                   || p.BuyerLastName.Contains(term)).ToList();
            return result;
        }

        public ProductBoughtReadDto FindProductByIdDtos(int ProductId)
        {
            return db.AssociatedBoughtSouth.Select(x => new ProductBoughtReadDto
            {
                BuyerId = x.Buyer.Id,
                product = x.product,
                BuyerFirstName = x.Buyer.FirstName,
                BuyerLastName = x.Buyer.LastName,
                BuyerEmail = x.Buyer.Email
            }).SingleOrDefault(p => p.product.ProductId == ProductId);
        }
        public List<ProductBoughtReadDto> ListDtos()
        {
            return db.AssociatedBoughtSouth.Select(x => new ProductBoughtReadDto
            {
                BuyerId = x.Buyer.Id,
                product = x.product,
                BuyerFirstName = x.Buyer.FirstName,
                BuyerLastName = x.Buyer.LastName,
                BuyerEmail = x.Buyer.Email
            }).ToList();
        }

        public bool IsUserBuyThis(string accountId, int productId)
        {
            if (db.AssociatedBoughtSouth.Where(p => p.product.ProductId == productId).Where(s => s.Buyer.Id == accountId) != null)
                return true;
            else
                return false;
        }

        public bool IsUserShareThis(string accountId, int productId)
        {
            throw new System.NotImplementedException();
        }
    }
}
