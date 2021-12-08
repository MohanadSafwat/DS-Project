using System.Collections.Generic;

namespace MarketPlace.Models.Repositories
{
    public interface IAssociatedRepository<TEnity>
    {
        IList<TEnity> List();


        TEnity Find(int ProductId);
        void Add(TEnity entity);
        void Edit(TEnity entity);
        void Delete(int ProductId);
        List<TEnity> FindProducts(string sellerId);
        
        int IsExist(TEnity entity);

        

    }
}
