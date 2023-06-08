using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TodoAPP.Core.Repositories;
using TodoAPP.Core.Services;
using TodoAPP.Core.UnitOfWorks;

namespace TodoAPP.Service.Services;

public class Service<T> : IService<T> where T :class
{
    private readonly IGenericRepository<T?> _repository;
    private readonly IUnitOfWork _unitOfWork;


    public Service(IGenericRepository<T?> repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }
    
    public IQueryable<T?> Where(Expression<Func<T?, bool>> expression)
    {
        var entities = _repository.Where(expression);
        return entities;
    }
    
    public async Task<bool> Update(T? entity)
    {
        _repository.Update(entity);
        await _unitOfWork.CommitAsync();
        return true;

    }
    
}