using TodoAPP.Core;
using TodoAPP.Core.DTOs;
using TodoAPP.Core.Models;
using TodoAPP.Core.Repositories;
using TodoAPP.Core.Services;
using TodoAPP.Core.UnitOfWorks;

namespace TodoAPP.Service.Services;

public class UserService:Service<UserEntity>,IUserService
{
    
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;
    private readonly IAuthenticationService _authenticationService;
    public UserService(IGenericRepository<UserEntity?> repository, IUnitOfWork unitOfWork, IUserRepository userRepository, IAuthenticationService authenticationService) : base(repository, unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
        _authenticationService = authenticationService;
    }
    
    public async Task<CustomResponseDto<UserEntity>> GetUserInfoByEmail(string email)
    {
        var user =  await _userRepository.GetUserInfoByEmail(email);
        
        if (user == null)
            return CustomResponseDto<UserEntity>.Fail(ResponseMessages.UserNotFound,404);
        
        return CustomResponseDto<UserEntity>.Success(user,200);

    }
    
    public async Task<CustomResponseDto<UserEntity>> RemoveAsync(UserEntity userEntity)
    {
        // Sets user isDeleted property to true
 
        userEntity.IsDeleted = true;
        await Update(userEntity);
        return CustomResponseDto<UserEntity>.Success(userEntity, 204);
    }

    public async Task<CustomResponseDto<UserEntity>> UpdateUser(UserUpdateDto userUpdateDto, SessionEntity? session)
    {

        if (session == null)
            return CustomResponseDto<UserEntity>.Fail(ResponseMessages.SessionNotFound,404);

        if (userUpdateDto.Password != null)
        {
            
            var userUpdateDtoPasswordHash = _authenticationService.CalculateHash(userUpdateDto.Password, session.UserEntity.PasswordSalt);

            if (session.UserEntity.PasswordHash != userUpdateDtoPasswordHash)
            {
                session.UserEntity.PasswordSalt = PasswordHasher.GenerateSalt();
                session.UserEntity.PasswordHash = _authenticationService.CalculateHash(userUpdateDto.Password, session.UserEntity.PasswordSalt);

            }
        }
        
        if (userUpdateDto.UserName != null)
            session.UserEntity.UserName = userUpdateDto.UserName != session.UserEntity.UserName ? userUpdateDto.UserName : session.UserEntity.UserName;

        if (userUpdateDto.Email != null)
            session.UserEntity.Email = userUpdateDto.Email != session.UserEntity.Email ? userUpdateDto.Email : session.UserEntity.Email;


        
        await Update(session.UserEntity);
        
        return CustomResponseDto<UserEntity>.Success(session.UserEntity ,200 );    
    }
}