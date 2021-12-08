using System.Collections.Generic;

namespace MarketPlace.Models.Repositories
{
    public interface IAssociatedRepository<TEnity>
    {
        IList<TEnity> List();


        TEnity Find(int ProductId, string SellerId);
        void Add(TEnity entity);
        void Edit(TEnity entity);
        void Delete(int ProductId, string SellerId);

        int IsExist(TEnity entity);

        

    }
}
