using System.Collections.Generic;
using System.Linq;

namespace MarketPlace.Models.Repositories
{
    public class ProductRepository : IProductRepository<Product>
    {
         List<Product> products;

        public ProductRepository()
        {
            products = new List<Product>() { 
            new Product{ 
                ProductId =1,
                ProductBrand = "Apple",
                ProductDescription ="Good Product",
                SellerId = 1,
                ProductPrice = 15000,
                ProductName = "iPhone 13 mini",
                ProductImageUrls= "image-1.jpg`image-2.jpg`image-3.jpg",
                Sold=true
            },
            new Product{
                ProductId =2,
                ProductBrand = "Apple",
                ProductDescription ="Good Product",
                SellerId = 1,
                ProductPrice = 13000,
                ProductName = "iPhone 13",
                ProductImageUrls= "image-1.jpg`image-2.jpg`image-3.jpg",
                Sold =false

            },  new Product{
                ProductId =3,
                ProductBrand = "Apple",
                ProductDescription ="Good Product",
                ProductPrice = 18000,
                ProductName = "iPhone 13 Pro",
                SellerId = 1,
                ProductImageUrls= "image-1.jpg`image-2.jpg`image-3.jpg",
                Sold=true

            },

            };

        }

        public void Add(Product entity)
        {
            entity.ProductId = products.Max(p=>p.ProductId) + 1;
            products.Add(entity);
        }

        public void Delete(int id)
        {
            var removedProduct = Find(id);
            products.Remove(removedProduct);
        }
      
        public void Edit(int id, Product entity)
        {
            var editedProduct = Find(id);
            editedProduct.ProductDescription = entity.ProductDescription;
            editedProduct.ProductName = entity.ProductName;
            editedProduct.ProductPrice = entity.ProductPrice;
            editedProduct.ProductBrand = entity.ProductBrand;
            editedProduct.ProductImageUrls = entity.ProductImageUrls;
            

            
        }

        public int IsExist(Product entity)
        {
            var product = products.SingleOrDefault(p =>
             p.ProductName == entity.ProductName && p.ProductDescription == entity.ProductDescription
             && p.ProductPrice == entity.ProductPrice && p.ProductBrand == entity.ProductBrand
            );
            if(product==null)
                return -1;
            else
                return product.ProductId;
        }
        public Product Find(int id)
        {
            return products.SingleOrDefault(p=>p.ProductId == id);
        }

        public IList<Product> List()
        {
            return products;
        }
    }
}
