using System.Linq.Expressions;

namespace TodoAPP.Core.Services;

public interface IService<T> where T:class
{
    
    Task<IEnumerable<T>> GetAll();
    Task<T> GetByIdAsync(int id);
    IQueryable<T> Where(Expression<Func<T, bool>> expression);
    Task<bool> AnyAsync(Expression<Func<T, bool>> expression);
    Task AddAsync(T entity);
    void Update(T entity);
    void Remove(T entity);
}