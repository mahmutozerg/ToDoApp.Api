using System.Linq.Expressions;

namespace TodoAPP.Core.Services;

public interface IService<T> where T:class
{
    IQueryable<T?> Where(Expression<Func<T?, bool>> expression);
    Task<bool> Update(T? entity);
}