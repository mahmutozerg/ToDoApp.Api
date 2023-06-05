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


    public async  Task<IEnumerable<T?>> GetAll()
    {
        return  await _repository.GetAll().ToListAsync();
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public IQueryable<T?> Where(Expression<Func<T?, bool>> expression)
    {
        return _repository.Where(expression);
    }

    public async Task<bool> AnyAsync(Expression<Func<T?, bool>> expression)
    {
        return await _repository.AnyAsync(expression);
    }

    public async Task AddAsync(T? entity)
    {
        await _repository.AddAsync(entity);
        await _unitOfWork.CommitAsync();
    }

    public async Task<bool> Update(T? entity)
    {
        _repository.Update(entity);
        await _unitOfWork.CommitAsync();
        return true;

    }

    public async void Remove(T? entity)
    {
        _repository.Remove(entity);
        await _unitOfWork.CommitAsync();

    }
}