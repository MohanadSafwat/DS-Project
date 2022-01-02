using JWTAuthentication.Authentication;
using MarketPlace.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MarketPlace.Models.Repositories
{
    public class AssociatedSellRepository : IAssociatedRepository<AssociatedSell, ProductSellerReadDto>
    {
        ApplicationDbContext db;

        public AssociatedSellRepository(ApplicationDbContext _db)
        {
            db = _db;
        }
        public void Add(AssociatedSell entity)
        {
            db.AssociatedSellUnSold.Add(entity);
            db.SaveChanges();
        }
        public int IsExist(AssociatedSell entity)
        {
            return 0;
        }
        public void Delete(int ProductId)
        {
            var associatedSell = Find(ProductId);
            db.AssociatedSellUnSold.Remove(associatedSell);
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
        public List<AssociatedSell> Search(string term)
        {
            var result = db.AssociatedSellUnSold.Include(p => p.productId).Include(s => s.SellerId).Where(p => p.productId.ProductName.Contains(term)
               || p.productId.ProductBrand.Contains(term) || p.productId.ProductDescription.Contains(term) || p.SellerId.FirstName.Contains(term)
                   || p.SellerId.LastName.Contains(term)).ToList();
            return result;
        }
        public List<AssociatedSell> FindUsers(int productId)
        {

            var resultUnSold = db.AssociatedSellUnSold.Include(p => p.productId).Include(s => s.SellerId).Where(p => p.productId.ProductId == productId).ToList();
            var resultSold = new List<AssociatedSell>();
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
        public AssociatedSell Find(int ProductId)
        {
            var product = db.AssociatedSellUnSold.Include(p => p.productId).Include(s => s.SellerId).SingleOrDefault(p => p.productId.ProductId == ProductId);
            return product;


        }

        public List<AssociatedSell> FindProducts(string sellerId)
        {
            var resultUnSold = db.AssociatedSellUnSold.Include(p => p.productId).Include(s => s.SellerId).Where(s => s.SellerId.Id == sellerId).ToList();
            var resultSold = new List<AssociatedSell>();

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

        public List<AssociatedSell> List()
        {
            var resultUnSold = db.AssociatedSellUnSold.Include(s => s.SellerId).Include(p => p.productId).ToList();
            var resultSold = new List<AssociatedSell>();

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




        public List<ProductSellerReadDto> SearchDtos(string term)
        {

            var result = db.AssociatedSellUnSold.Select(x => new ProductSellerReadDto
            {
                sellerId = x.SellerId.Id,
                product = x.productId,
                sellerFirstName = x.SellerId.FirstName,
                sellerLastName = x.SellerId.LastName,
                sellerEmail = x.SellerId.Email
            }).Where(p => p.product.ProductName.Contains(term)
|| p.product.ProductBrand.Contains(term) || p.product.ProductDescription.Contains(term) || p.sellerFirstName.Contains(term)
|| p.sellerLastName.Contains(term)).ToList();
            return result;
        }
        public List<ProductSellerReadDto> FindUsersDtos(int productId)
        {
            var resultUnSold = db.AssociatedSellUnSold.Select(x => new ProductSellerReadDto
            {
                sellerId = x.SellerId.Id,
                product = x.productId,
                sellerFirstName = x.SellerId.FirstName,
                sellerLastName = x.SellerId.LastName,
                sellerEmail = x.SellerId.Email
            }).Where(p => p.product.ProductId == productId).ToList();
            var resultSold = new List<ProductSellerReadDto>();

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
        public ProductSellerReadDto FindProductByIdDtos(int ProductId)
        {
            var product = db.AssociatedSellUnSold.Select(x => new ProductSellerReadDto
            {
                sellerId = x.SellerId.Id,
                product = x.productId,
                sellerFirstName = x.SellerId.FirstName,
                sellerLastName = x.SellerId.LastName,
                sellerEmail = x.SellerId.Email
            }).SingleOrDefault(p => p.product.ProductId == ProductId);
            return product;

        }


        public List<ProductSellerReadDto> FindProductsDtos(string sellerId)
        {
            var resultUnSold = db.AssociatedSellUnSold.Where(s => s.SellerId.Id == sellerId).Select(x => new ProductSellerReadDto
            {
                sellerId = x.SellerId.Id,
                product = x.productId,
                sellerFirstName = x.SellerId.FirstName,
                sellerLastName = x.SellerId.LastName,
                sellerEmail = x.SellerId.Email
            }).ToList();
            var resultSold = new List<ProductSellerReadDto>();


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


        public List<ProductSellerReadDto> FindUnSoldProductsDtos(string sellerId)
        {
            return db.AssociatedSellUnSold.Where(s => s.SellerId.Id == sellerId && !s.Sold).Select(x => new ProductSellerReadDto
            {
                sellerId = x.SellerId.Id,
                product = x.productId,
                sellerFirstName = x.SellerId.FirstName,
                sellerLastName = x.SellerId.LastName,
                sellerEmail = x.SellerId.Email
            }).ToList();
        }
        public List<ProductSellerReadDto> FindSoldProductsDtos(string sellerId)
        {
            return db.AssociatedSellUnSold.Where(s => s.SellerId.Id == sellerId && s.Sold).Select(x => new ProductSellerReadDto
            {
                sellerId = x.SellerId.Id,
                product = x.productId,
                sellerFirstName = x.SellerId.FirstName,
                sellerLastName = x.SellerId.LastName,
                sellerEmail = x.SellerId.Email
            }).ToList();
        }

        public List<ProductSellerReadDto> ListDtos()
        {
            var resultUnSold = db.AssociatedSellUnSold.Select(x => new ProductSellerReadDto
            {
                sellerId = x.SellerId.Id,
                product = x.productId,
                sellerFirstName = x.SellerId.FirstName,
                sellerLastName = x.SellerId.LastName,
                sellerEmail = x.SellerId.Email,
                Sold = x.Sold


            }).ToList();
            var resultSold = new List<ProductSellerReadDto>();


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
            throw new NotImplementedException();
        }

        public bool IsUserShareThis(string accountId, int productId)
        {
            throw new NotImplementedException();
        }
    }
}