using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPlace.Models.Repositories
{
    public class ProductDbRepository : IProductRepository<Product>
    {
        AppDBContext db;
        private readonly string _connectionString;

        public ProductDbRepository(AppDBContext _db , IOptions<AppDbConnection> config)
        {
            db = _db;
            _connectionString = config.Value.ConnectionString;
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

        public List<Product> List()
        {
            return db.Products.ToList() ;
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            using(IDbConnection db = new SqlConnection(_connectionString))
            {
                return  await db.QueryAsync<Product>("select ProductId,ProductName,ProductPrice,ProductBrand,ProductDescription from Products", commandType: CommandType.Text);
            }
        }
    }
}


