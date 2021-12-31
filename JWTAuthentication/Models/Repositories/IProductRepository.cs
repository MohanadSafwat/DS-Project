using System.Collections.Generic;

namespace MarketPlace.Models.Repositories
{
    public interface IProductRepository<TEnity>
    {
        List<TEnity> List(string Location);


        TEnity Find(int id,string Location);
        void Add(TEnity entity,string Location);
        void Edit(int id,TEnity entity,string Location);
        void Delete(int id,string Location);

        int IsExist(TEnity entity,string Location);

        

    }
}
