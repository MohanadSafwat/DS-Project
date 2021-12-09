using System.Collections.Generic;
using System.Threading.Tasks;

namespace MarketPlace.Models.Repositories
{
    public interface IProductRepository<TEnity>
    {


        Task<IEnumerable<Product>> GetProducts();

        List<TEnity> List();


        TEnity Find(int id);
        void Add(TEnity entity);
        void Edit(int id,TEnity entity);
        void Delete(int id);

        int IsExist(TEnity entity);

        

    }
}
