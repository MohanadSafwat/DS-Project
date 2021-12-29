using System.Collections.Generic;

namespace MarketPlace.Models.Repositories
{
    public interface IAssociatedRepository<TEnity>
    {
        List<TEnity> List();


        TEnity Find(int ProductId);
        void Add(TEnity entity);
        void Edit(TEnity entity);
        void Delete(int ProductId);
        List<TEnity> FindProducts(string sellerId);
        List<TEnity> FindUsers(int productId);
        public void EditList(List<TEnity> entityList);
        
        int IsExist(TEnity entity);

        public List<TEnity> Search(string term);

        

    }
}
