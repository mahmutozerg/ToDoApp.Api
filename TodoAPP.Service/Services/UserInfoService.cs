using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using TodoAPP.Core.DTOs;
using TodoAPP.Core.Models;
using TodoAPP.Core.Repositories;
using TodoAPP.Core.Services;
using TodoAPP.Core.UnitOfWorks;
using TodoAPP.Repository.Repositories;

namespace TodoAPP.Service.Services;

public class UserInfoService:Service<UserInfo>,IUserInfoService
{
    
    private readonly string _pepper;
    private readonly int _iteration = 5;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserInfoRepository _userInfoRepository;
    public UserInfoService(IGenericRepository<UserInfo?> repository, IUnitOfWork unitOfWork, IUserInfoRepository userInfoRepository) : base(repository, unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _userInfoRepository = userInfoRepository;
        _pepper = "Mahmut";
    }
    public async Task<string> Register(UserDto userDto,UserInfoDetailed userInfoDetailed)
    {
        var errorMessage = await CheckUserExist(userDto);

        if (string.IsNullOrEmpty(errorMessage))
        {
            var newUser = new UserInfo()
            {
                UserName = userDto.UserName,
                Email = userDto.Email,
                PasswordSalt = PasswordHasher.GenerateSalt(),
                IsDeleted = false,
                UserInfoDetailed = userInfoDetailed,
                ToDos = new List<ToDo>()

            };
            newUser.PasswordHash= PasswordHasher.ComputeHash(userDto.Password, newUser.PasswordSalt, _pepper, _iteration);
        
            await _userInfoRepository.AddAsync(newUser);
            await _unitOfWork.CommitAsync();
            return string.Empty;
        }
        
        return errorMessage;

    }
    
    public async Task<string> Login(UserDto userDto)
    {
        var user = await _userInfoRepository.GetUserInfoWithDetailsByEmail(userDto.Email);

        if (user.UserInfoDetailed.SessionGUID !=string.Empty)
        {
            return "User already logged in";
        }

        var passwordHash = PasswordHasher.ComputeHash(userDto.Password, user!.PasswordSalt, _pepper, _iteration);

        if (passwordHash != user.PasswordHash)
        {
            return "Please check your credentials";
        }


        return string.Empty;

    }

    public async Task<string> CheckUserExist(UserDto userDto)
    {
        var errorMessage = string.Empty;

        var isUsernameExist = await _userInfoRepository.Where(x => x.UserName == userDto.UserName).AsNoTracking().AnyAsync();
        var isEmailExist = await _userInfoRepository.Where(x => x.Email == userDto.Email).AsNoTracking().AnyAsync();
        
        var isUserDeleted =  await _userInfoRepository.Where(x=> (x.UserName ==  userDto.UserName || x.Email == userDto.Email)).
            Where(x=> x.IsDeleted ).AnyAsync();

        if ((!isUsernameExist  && !isEmailExist ) || isUserDeleted) 
            return errorMessage;
        
        
        
        errorMessage += isEmailExist ? "Email already exist " :  string.Empty;
        errorMessage += isUsernameExist ? "User name already exist " :  string.Empty;


        return errorMessage;
    }

   
    public async Task<UserInfo?> GetUserInfoWithDetailsByEmail(string email)
    {
        var entity =  await _userInfoRepository.GetUserInfoWithDetailsByEmail(email);
        return entity;
    }

    public async Task<UserInfo?> GetUserInfoWithDetailsBySessionId(string sessionId)
    {
        var entity=  await _userInfoRepository.GetUserInfoWithDetailsBySessionId(sessionId);
        return entity;
    }

    public async Task<UserInfo?> Remove(string email,UserInfo userInfo)
    {
        if (userInfo != null)
        {
            userInfo.IsDeleted = true;
            await Update(userInfo);
        }

        return userInfo;    
    }


    
   

}