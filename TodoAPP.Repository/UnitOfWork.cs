using Microsoft.EntityFrameworkCore;
using TodoAPP.Core.UnitOfWorks;

namespace TodoAPP.Repository;

public class UnitOfWork:IUnitOfWork
{

    private readonly DbContext _context;

    public UnitOfWork(DbContext context)
    {
        _context = context;
    }

    public async Task CommitAsync()
    {
        await _context.SaveChangesAsync();
    }

    public void Commit()
    {
        _context.SaveChanges();
    }
}

