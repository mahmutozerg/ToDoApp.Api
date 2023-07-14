using System.Linq.Expressions;
using TodoAPP.Auth.Core.Models;

namespace TodoAPP.Auth.Core.Services;

public interface IGenericService<TEntity> where TEntity:class
{

    IQueryable<TEntity?> Where(Expression<Func<TEntity?, bool>> expression);
    Task<bool> Update(TEntity? entity);

    Task Remove(TEntity entity);
}