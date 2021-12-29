/*using System.Collections.Generic;
using System.Linq;

namespace MarketPlace.Models.Repositories
{
    public class ProductRepository : IProductRepository<Product>
    {
         List<Product> products;

        public ProductRepository() { 
        

            
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
*/