using JWTAuthentication.Authentication;
using MarketPlace.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MarketPlace.Models.Repositories
{
    public class SouthAssociatedSellRepository : IAssociatedRepository<AssociatedSellSouth, ProductSellerReadDto>
    {
        AppDB2Context db;

        public SouthAssociatedSellRepository(AppDB2Context _db)
        {
            db = _db;
        }
        public void Add(AssociatedSellSouth entity)
        {
            db.AssociatedSellSouthUnSold.Add(entity);
            db.SaveChanges();
        }
        public int IsExist(AssociatedSellSouth entity)
        {
            return 0;
        }
        public void Delete(int ProductId)
        {
            var AssociatedSellSouth = Find(ProductId);
            db.AssociatedSellSouthUnSold.Remove(AssociatedSellSouth);
            db.SaveChanges();
        }

        public void Edit(AssociatedSellSouth entity)
        {

            db.Update(entity);
            db.SaveChanges();
        }
        public void EditList(List<AssociatedSellSouth> entityList)
        {

            db.Update(entityList);
            db.SaveChanges();
        }
        public List<AssociatedSellSouth> Search(string term)
        {
            var result = db.AssociatedSellSouthUnSold.Include(p => p.productId).Include(s => s.SellerId).Where(p => p.productId.ProductName.Contains(term)
               || p.productId.ProductBrand.Contains(term) || p.productId.ProductDescription.Contains(term) || p.SellerId.FirstName.Contains(term)
                   || p.SellerId.LastName.Contains(term)).ToList();
            return result;
        }
        public List<AssociatedSellSouth> FindUsers(int productId)
        {

            var resultUnSold = db.AssociatedSellSouthUnSold.Include(p => p.productId).Include(s => s.SellerId).Where(p => p.productId.ProductId == productId).ToList();
            var resultSold = new List<AssociatedSellSouth>();
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
        public AssociatedSellSouth Find(int ProductId)
        {
            var product = db.AssociatedSellSouthUnSold.Include(p => p.productId).Include(s => s.SellerId).SingleOrDefault(p => p.productId.ProductId == ProductId);
            return product;


        }

        public List<AssociatedSellSouth> FindProducts(string sellerId)
        {
            var resultUnSold = db.AssociatedSellSouthUnSold.Include(p => p.productId).Include(s => s.SellerId).Where(s => s.SellerId.Id == sellerId).ToList();
            var resultSold = new List<AssociatedSellSouth>();

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

        public List<AssociatedSellSouth> List()
        {
            var resultUnSold = db.AssociatedSellSouthUnSold.Include(s => s.SellerId).Include(p => p.productId).ToList();
            var resultSold = new List<AssociatedSellSouth>();

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

            var result = db.AssociatedSellSouthUnSold.Select(x => new ProductSellerReadDto
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
            var resultUnSold = db.AssociatedSellSouthUnSold.Select(x => new ProductSellerReadDto
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
            var product = db.AssociatedSellSouthUnSold.Select(x => new ProductSellerReadDto
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
            var resultUnSold = db.AssociatedSellSouthUnSold.Where(s => s.SellerId.Id == sellerId).Select(x => new ProductSellerReadDto
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
            return db.AssociatedSellSouthUnSold.Where(s => s.SellerId.Id == sellerId && (!s.Sold)).Select(x => new ProductSellerReadDto
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
            return db.AssociatedSellSouthUnSold.Where(s => (s.SellerId.Id == sellerId) && (s.Sold)).Select(x => new ProductSellerReadDto
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
            var resultUnSold = db.AssociatedSellSouthUnSold.Select(x => new ProductSellerReadDto
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