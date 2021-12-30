using MarketPlace.Dtos;
using System;
using System.Collections.Generic;

namespace MarketPlace.Models.Repositories
{
    public interface IAssociatedRepository<TEntity,TDtos>
    {
        List<TDtos> List();


        TEntity Find(int ProductId);
        TDtos FindProductById(int ProductId);

        void Add(TEntity entity);
        void Edit(TEntity entity);
        void Delete(int ProductId);

        List<TDtos> FindProducts(string sellerId);
        List<TDtos> FindUsers(int productId);
        public void EditList(List<TEntity> entityList);
        
        int IsExist(TEntity entity);

        public List<TDtos> Search(string term);

        

    }
}
