using MarketPlace.Dtos;
using System;
using System.Collections.Generic;

namespace MarketPlace.Models.Repositories
{
    public interface IAssociatedRepository<TEntity, TDtos>
    {
        List<TDtos> ListDtos();
        TDtos FindProductByIdDtos(int ProductId);
        List<TDtos> FindProductsDtos(string accountId);
        List<TDtos> FindUnSoldProductsDtos(string accountId);
        List<TDtos> FindSoldProductsDtos(string accountId);
        List<TDtos> FindUsersDtos(int productId);
         List<TDtos> SearchDtos(string term);
        List<TEntity> List();
        TEntity Find(int ProductId);
        void Add(TEntity entity);
        void Edit(TEntity entity);
        void Delete(int ProductId);
        List<TEntity> FindProducts(string accountId);
        List<TEntity> FindUsers(int productId);
         void EditList(List<TEntity> entityList);
        int IsExist(TEntity entity);
         List<TEntity> Search(string term);
    }
}
