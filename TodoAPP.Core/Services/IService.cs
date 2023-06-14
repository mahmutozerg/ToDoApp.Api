using System.Linq.Expressions;

namespace TodoAPP.Core.Services;

public interface IService<T> where T:class
{
    // todo: async methodlar her zaman Async olarak bitmeli
    //todo : async methodlar her aman cancellation token almalı
    IQueryable<T?> Where(Expression<Func<T?, bool>> expression);
    Task<bool> Update(T? entity);
}