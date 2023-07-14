using System.Linq.Expressions;

namespace TodoAPP.Core.Services;

public interface IService<TEntity> where TEntity:class
{
    // todo: async methodlar her zaman Async olarak bitmeli
    //todo : async methodlar her aman cancellation token almalı
    IQueryable<TEntity?> Where(Expression<Func<TEntity?, bool>> expression);
    Task<bool> Update(TEntity? entity);
}