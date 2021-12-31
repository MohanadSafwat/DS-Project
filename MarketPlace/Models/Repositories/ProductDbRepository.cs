using JWTAuthentication.Authentication;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace MarketPlace.Models.Repositories
{
    public class ProductDbRepository : IProductRepository<Product>
    {
        AppDBContext db;
        AppDB2Context db2;

        public ProductDbRepository(AppDBContext _db, AppDB2Context _db2)
        {
            db = _db;
            db2 = _db2;

        }

        public void Add(Product entity, string Location)
        {
            if (Location == "North")
            {
                db.Products.Add(entity);
                db.SaveChanges();
            }
            else
            {
                db2.Products.Add(entity);
                db2.SaveChanges();
            }

        }

        public void Delete(int id, string Location)
        {
            if (Location == "North")
            {
                var removedProduct = Find(id, Location);
                db.Products.Remove(removedProduct);
                db.SaveChanges();
            }
            else
            {
                var removedProduct = Find(id, Location);
                db2.Products.Remove(removedProduct);
                db2.SaveChanges();
            }


        }

        public void Edit(int id, Product entity, string Location)
        {
            /*var editedProduct = Find(id);
            editedProduct.ProductDescription = entity.ProductDescription;
            editedProduct.ProductName = entity.ProductName;
            editedProduct.ProductPrice = entity.ProductPrice;
            editedProduct.ProductBrand = entity.ProductBrand;
            editedProduct.ProductImageUrls = entity.ProductImageUrls;*/
            if (Location == "North")
            {
                db.Update(entity);
                db.SaveChanges();
            }
            else
            {
                db2.Update(entity);
                db2.SaveChanges();
            }
        }

        public int IsExist(Product entity, string Location)
        {
            if (Location == "North")
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
            else
            {
                var product = db2.Products.SingleOrDefault(p =>
          p.ProductName == entity.ProductName && p.ProductDescription == entity.ProductDescription
          && p.ProductPrice == entity.ProductPrice && p.ProductBrand == entity.ProductBrand
         );
                if (product == null)
                    return -1;
                else
                    return product.ProductId;
            }

        }
        public Product Find(int id, string Location)
        {
            if (Location == "North")
            {
                return db.Products.SingleOrDefault(p => p.ProductId == id);

            }
            else
            {
                return db2.Products.SingleOrDefault(p => p.ProductId == id);

            }
        }

        public List<Product> List(string Location)
        {
            if (Location == "North")
            {
                return db.Products.ToList();
            }
            else
            {
                return db2.Products.ToList();
            }
        }
    }
}


