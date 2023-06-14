using TodoAPP.Core;
using TodoAPP.Core.DTOs;
using TodoAPP.Core.Models;
using TodoAPP.Core.Repositories;
using TodoAPP.Core.Services;
using TodoAPP.Core.UnitOfWorks;

namespace TodoAPP.Service.Services;

public class UserService:Service<User>,IUserService
{
    
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;
    private readonly IAuthenticationService _authenticationService;
    public UserService(IGenericRepository<User?> repository, IUnitOfWork unitOfWork, IUserRepository userRepository, IAuthenticationService authenticationService) : base(repository, unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
        _authenticationService = authenticationService;
    }
    
    public async Task<User?> GetUserInfoByEmail(string email)
    {
        return await _userRepository.GetUserInfoByEmail(email);
    }
    
    public async Task<User?> RemoveAsync(User user)
    {
        // Sets user isDeleted property to true
 
        user.IsDeleted = true;
        await Update(user);
        return user;    
    }

    public async Task<string> UpdateUser(UserUpdateDto userUpdateDto, Session? session)
    {

        if (session == null)
            return ResponseMessages.SessionNotFound;

        if (userUpdateDto.Password != null)
        {
            
            var userUpdateDtoPasswordHash = _authenticationService.CalculateHash(userUpdateDto.Password, session.User.PasswordSalt);

            if (session.User.PasswordHash != userUpdateDtoPasswordHash)
            {
                session.User.PasswordSalt = PasswordHasher.GenerateSalt();
                session.User.PasswordHash = _authenticationService.CalculateHash(userUpdateDto.Password, session.User.PasswordSalt);

            }
        }
        
        if (userUpdateDto.UserName != null)
            session.User.UserName = userUpdateDto.UserName != session.User.UserName ? userUpdateDto.UserName : session.User.UserName;

        if (userUpdateDto.Email != null)
            session.User.Email = userUpdateDto.Email != session.User.Email ? userUpdateDto.Email : session.User.Email;


        
        await Update(session.User);
        
        return string.Empty;    
    }
}