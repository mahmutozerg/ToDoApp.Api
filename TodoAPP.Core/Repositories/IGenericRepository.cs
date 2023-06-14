using System.Linq.Expressions;

namespace TodoAPP.Core.Repositories;

public interface IGenericRepository<T> where T:class?
{
    IQueryable<T?> Where(Expression<Func<T?, bool>> expression);
    Task<bool> AnyAsync(Expression<Func<T?, bool>> expression);
    Task AddAsync(T? entity);
    void Update(T? entity);

}