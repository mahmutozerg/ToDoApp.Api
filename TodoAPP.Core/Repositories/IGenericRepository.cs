using System.Linq.Expressions;

namespace TodoAPP.Core.Repositories;

public interface IGenericRepository<T> where T:class
{
    Task<T?> GetByIdAsync(int id);
    IQueryable<T?> Where(Expression<Func<T?, bool>> expression);
    Task AddAsync(T? entity);
    void Update(T? entity);

}