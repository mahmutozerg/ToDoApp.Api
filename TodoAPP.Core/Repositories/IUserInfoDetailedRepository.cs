using TodoAPP.Core.Models;

namespace TodoAPP.Core.Repositories;

public interface IUserInfoDetailedRepository:IGenericRepository<UserInfoDetailed>
{
    public  Task<UserInfoDetailed> Create( );

}