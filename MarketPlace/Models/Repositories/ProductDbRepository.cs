using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace MarketPlace.Models.Repositories
{
    public class ProductDbRepository : IProductRepository<Product>
    {
        MarketPlaceDbContext db;

        public ProductDbRepository(MarketPlaceDbContext _db)
        {
            db = _db;
        }

        public void Add(Product entity)
        {
            db.Products.Add(entity);
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            var removedProduct = Find(id);
            db.Products.Remove(removedProduct);
            db.SaveChanges();

        }

        public void Edit(int id, Product entity)
        {
            /*var editedProduct = Find(id);
            editedProduct.ProductDescription = entity.ProductDescription;
            editedProduct.ProductName = entity.ProductName;
            editedProduct.ProductPrice = entity.ProductPrice;
            editedProduct.ProductBrand = entity.ProductBrand;
            editedProduct.ProductImageUrls = entity.ProductImageUrls;*/

            db.Update(entity);
            db.SaveChanges();


        }

        public int IsExist(Product entity)
        {
            var product = db.Products.SingleOrDefault(p =>
             p.ProductName == entity.ProductName && p.ProductDescription == entity.ProductDescription
             && p.ProductPrice == entity.ProductPrice && p.ProductBrand == entity.ProductBrand
            );
            if (product == null)
                return -1;
            else
                return product.ProductId;
        }
        public Product Find(int id)
        {
            return db.Products.SingleOrDefault(p => p.ProductId == id);
        }

        public IList<Product> List()
        {
            return db.Products.ToList() ;
        }
    }
}


