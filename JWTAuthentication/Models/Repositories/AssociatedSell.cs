using JWTAuthentication.Authentication;
using MarketPlace.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MarketPlace.Models.Repositories
{
    public class AssociatedSellRepository : IAssociatedRepository<AssociatedSell,ProductSellerReadDto>
    {
        ApplicationDbContext db;

        public AssociatedSellRepository(ApplicationDbContext _db)
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
        public void Delete(int ProductId)
        {
            var associatedSell = Find(ProductId);
            db.AssociatedSell.Remove(associatedSell);
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
        public List<ProductSellerReadDto> Search(string term) {
            var result = db.AssociatedSell.Select(x => new ProductSellerReadDto{
                sellerId = x.SellerId.Id,
                product = x.productId,
                sellerFirstName = x.SellerId.FirstName,
                sellerLastName = x.SellerId.LastName,
                sellerEmail = x.SellerId.Email}).Where(p => p.product.ProductName.Contains(term)
               || p.product.ProductBrand.Contains(term) || p.product.ProductDescription.Contains(term) || p.sellerFirstName.Contains(term)
                   || p.sellerLastName.Contains(term)).ToList();
            return result;
        }
        public List<ProductSellerReadDto> FindUsers(int productId)
        {

            var result = db.AssociatedSell.Select(x => new ProductSellerReadDto{
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
        public ProductSellerReadDto FindProductById(int ProductId)
        {
            return db.AssociatedSell.Select(x => new ProductSellerReadDto{
                sellerId = x.SellerId.Id,
                product = x.productId,
                sellerFirstName = x.SellerId.FirstName,
                sellerLastName = x.SellerId.LastName,
                sellerEmail = x.SellerId.Email}).SingleOrDefault(p => p.product.ProductId == ProductId );
        }
           public AssociatedSell Find(int ProductId)
        {
            return db.AssociatedSell.Include(s=>s.SellerId).Include(p => p.productId).SingleOrDefault(p => p.productId.ProductId == ProductId );
        }

        public List<ProductSellerReadDto> FindProducts(string sellerId)
        {
            return db.AssociatedSell.Where(s => s.SellerId.Id == sellerId).Select(x => new ProductSellerReadDto{
                sellerId = x.SellerId.Id,
                product = x.productId,
                sellerFirstName = x.SellerId.FirstName,
                sellerLastName = x.SellerId.LastName,
                sellerEmail = x.SellerId.Email}).ToList();
        }

        public List<ProductSellerReadDto> List()
        {
            return db.AssociatedSell.Select(x => new ProductSellerReadDto
            {
                sellerId = x.SellerId.Id,
                product = x.productId,
                sellerFirstName = x.SellerId.FirstName,
                sellerLastName = x.SellerId.LastName,
                sellerEmail = x.SellerId.Email,
                Sold = x.Sold


            }).ToList();
        }

    }
}
