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
    
    public async Task<CustomResponseDto<User>> GetUserInfoByEmail(string email)
    {
        var user =  await _userRepository.GetUserInfoByEmail(email);
        
        if (user == null)
            return CustomResponseDto<User>.Fail(ResponseMessages.UserNotFound,404);
        
        return CustomResponseDto<User>.Success(user,200);

    }
    
    public async Task<CustomResponseDto<User>> RemoveAsync(User user)
    {
        // Sets user isDeleted property to true
 
        user.IsDeleted = true;
        await Update(user);
        return CustomResponseDto<User>.Success(user, 204);
    }

    public async Task<CustomResponseDto<User>> UpdateUser(UserUpdateDto userUpdateDto, Session? session)
    {

        if (session == null)
            return CustomResponseDto<User>.Fail(ResponseMessages.SessionNotFound,404);

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
        
        return CustomResponseDto<User>.Success(session.User ,200 );    
    }
}