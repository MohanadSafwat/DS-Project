using JWTAuthentication.Authentication;
using MarketPlace.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MarketPlace.Models.Repositories
{
    public class SouthAssociatedSellRepository : IAssociatedRepository<AssociatedSellSouth,ProductSellerReadDto>
    {
        ApplicationDb2Context db;

        public SouthAssociatedSellRepository(ApplicationDb2Context _db)
        {
            db = _db;
        }
        public void Add(AssociatedSellSouth entity)
        {
            db.AssociatedSellSouth.Add(entity);
            db.SaveChanges();
        }
        public int IsExist(AssociatedSellSouth entity)
        {
            return 0;
        }
        public void Delete(int ProductId)
        {
            var AssociatedSellSouth = Find(ProductId);
            db.AssociatedSellSouth.Remove(AssociatedSellSouth);
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
        public List<AssociatedSellSouth> Search(string term) {
            var result = db.AssociatedSellSouth.Include(p => p.productId).Include(s => s.SellerId).Where(p => p.productId.ProductName.Contains(term)
               || p.productId.ProductBrand.Contains(term) || p.productId.ProductDescription.Contains(term) || p.SellerId.FirstName.Contains(term)
                   || p.SellerId.LastName.Contains(term)).ToList();
            return result;
        }
        public List<AssociatedSellSouth> FindUsers(int productId)
        {

            var result = db.AssociatedSellSouth.Include(p => p.productId).Include(s => s.SellerId).Where(p => p.productId.ProductId == productId).ToList();
            if (result != null)
                return result;
            else
                return new List<AssociatedSellSouth>();
        }
        public AssociatedSellSouth Find(int ProductId)
        {
            return db.AssociatedSellSouth.Include(p=>p.productId).Include(s=>s.SellerId).SingleOrDefault(p => p.productId.ProductId == ProductId );
        }

        public List<AssociatedSellSouth> FindProducts(string sellerId)
        {
            return db.AssociatedSellSouth.Include(p => p.productId).Include(s => s.SellerId).Where(s => s.SellerId.Id == sellerId).ToList();
        }

        public List<AssociatedSellSouth> List()
        {
            return db.AssociatedSellSouth.Include(s => s.SellerId).Include(p => p.productId).ToList();
        }

    

    
        public List<ProductSellerReadDto> SearchDtos(string term) {
            var result = db.AssociatedSellSouth.Select(x => new ProductSellerReadDto{
                sellerId = x.SellerId.Id,
                product = x.productId,
                sellerFirstName = x.SellerId.FirstName,
                sellerLastName = x.SellerId.LastName,
                sellerEmail = x.SellerId.Email}).Where(p => p.product.ProductName.Contains(term)
               || p.product.ProductBrand.Contains(term) || p.product.ProductDescription.Contains(term) || p.sellerFirstName.Contains(term)
                   || p.sellerLastName.Contains(term)).ToList();
            return result;
        }
        public List<ProductSellerReadDto> FindUsersDtos(int productId)
        {

            var result = db.AssociatedSellSouth.Select(x => new ProductSellerReadDto{
                sellerId = x.SellerId.Id,
                product = x.productId,
                sellerFirstName = x.SellerId.FirstName,
                sellerLastName = x.SellerId.LastName,
                sellerEmail = x.SellerId.Email}).Where(p => p.product.ProductId == productId).ToList();
            if (result != null)
                return result;
            else
                return new List<ProductSellerReadDto>();
        }
        public ProductSellerReadDto FindProductByIdDtos(int ProductId)
        {
            return db.AssociatedSellSouth.Select(x => new ProductSellerReadDto{
                sellerId = x.SellerId.Id,
                product = x.productId,
                sellerFirstName = x.SellerId.FirstName,
                sellerLastName = x.SellerId.LastName,
                sellerEmail = x.SellerId.Email}).SingleOrDefault(p => p.product.ProductId == ProductId );
        }


        public List<ProductSellerReadDto> FindProductsDtos(string sellerId)
        {
            return db.AssociatedSellSouth.Where(s => s.SellerId.Id == sellerId).Select(x => new ProductSellerReadDto{
                sellerId = x.SellerId.Id,
                product = x.productId,
                sellerFirstName = x.SellerId.FirstName,
                sellerLastName = x.SellerId.LastName,
                sellerEmail = x.SellerId.Email}).ToList();
        }

        
        public List<ProductSellerReadDto> FindUnSoldProductsDtos(string sellerId)
        {
            return db.AssociatedSellSouth.Where(s => s.SellerId.Id == sellerId && !s.Sold).Select(x => new ProductSellerReadDto{
                sellerId = x.SellerId.Id,
                product = x.productId,
                sellerFirstName = x.SellerId.FirstName,
                sellerLastName = x.SellerId.LastName,
                sellerEmail = x.SellerId.Email}).ToList();
        }
        public List<ProductSellerReadDto> FindSoldProductsDtos(string sellerId)
        {
            return db.AssociatedSellSouth.Where(s => s.SellerId.Id == sellerId && s.Sold).Select(x => new ProductSellerReadDto{
                sellerId = x.SellerId.Id,
                product = x.productId,
                sellerFirstName = x.SellerId.FirstName,
                sellerLastName = x.SellerId.LastName,
                sellerEmail = x.SellerId.Email}).ToList();
        }

        public List<ProductSellerReadDto> ListDtos()
        {
            return db.AssociatedSellSouth.Select(x => new ProductSellerReadDto
            {
                sellerId = x.SellerId.Id,
                product = x.productId,
                sellerFirstName = x.SellerId.FirstName,
                sellerLastName = x.SellerId.LastName,
                sellerEmail = x.SellerId.Email,
                Sold = x.Sold


            }).ToList();
        }

        public bool IsUserBuyThis(string accountId,int productId)
        {
            throw new NotImplementedException();
        }

        public bool IsUserShareThis(string accountId,int productId)
        {
            throw new NotImplementedException();
        }
    }
}