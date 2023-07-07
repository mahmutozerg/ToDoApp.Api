using Microsoft.EntityFrameworkCore;
using TodoAPP.Core;
using TodoAPP.Core.DTOs;
using TodoAPP.Core.Models;
using TodoAPP.Core.Repositories;
using TodoAPP.Core.Services;
using TodoAPP.Core.UnitOfWorks;

namespace TodoAPP.Service.Services;

public class AuthenticationService:IAuthenticationService
{
    private readonly string _pepper;
    private readonly int _iteration;
    
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;
    private readonly ISessionService _sessionService;

    public AuthenticationService( IUnitOfWork unitOfWork, IUserRepository userRepository, ISessionService sessionService)
    {
        _pepper = "Mahmut!123";
        _iteration = 5;
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
        _sessionService = sessionService;
    }

    public string CalculateHash(string password, string salt)
    {
        return PasswordHasher.ComputeHash(password, salt, _pepper, _iteration);
    }

    
    public async Task<string> Register(UserDto userDto)
    {
        
        var userExistErrorMessage = await CheckUserExist(userDto);
        if (string.IsNullOrEmpty(userExistErrorMessage))
        {
            var newUser = new UserEntity()
            {
                UserName = userDto.UserName,
                Email = userDto.Email,
                PasswordSalt = PasswordHasher.GenerateSalt(),
                IsDeleted = false,
                ToDos = new List<ToDoEntity>()

            };
            newUser.PasswordHash= CalculateHash(userDto.Password, newUser.PasswordSalt);
            await _userRepository.AddAsync(newUser);
            await _unitOfWork.CommitAsync();
            var session = await _sessionService.CreateSession(newUser.Id);

            return session.SessionGUID;
        }
        
        return string.Empty;
    }

    public async Task<SessionEntity?> Login(UserDto userDto)
    {

        var user = await _userRepository.GetUserInfoByEmail(userDto.Email);
        var passwordHash = PasswordHasher.ComputeHash(userDto.Password, user!.PasswordSalt, _pepper, _iteration);

        if (passwordHash != user.PasswordHash)
        {
            return null;
        }

        var session = await _sessionService.CreateSession(user.Id);
        return session;

    }
    
    private async Task<string> CheckUserExist(UserDto userDto)
    {
        var userExistErrorMessage = string.Empty;
        var isUsernameExist = await _userRepository.AnyAsync(x => x!.UserName == userDto.UserName && !x.IsDeleted);
        var isEmailExist = await _userRepository.AnyAsync(x => x!.Email == userDto.Email && !x.IsDeleted);
        
        if (!isUsernameExist  && !isEmailExist ) 
            return userExistErrorMessage;
        
        userExistErrorMessage += isEmailExist ? ResponseMessages.EmailExist :  string.Empty;
        userExistErrorMessage += isUsernameExist ? ResponseMessages.UsernameExist :  string.Empty;
        
        return userExistErrorMessage;
    }

    public async Task<SessionEntity> Logout(SessionGuiddto sessionDto)
    {
        
        var session = await _sessionService.Where(s => s!.SessionGUID == sessionDto.SessionGUID && s.IsSessionActive).FirstOrDefaultAsync();

        if ((session == null || string.IsNullOrEmpty(session.SessionGUID)))
            throw new Exception(ResponseMessages.SessionNotFound);
     
        session.IsSessionActive = false;
        session.EndedAt = DateTime.Now;
        await _sessionService.Update(session);

        return session;

    }


}