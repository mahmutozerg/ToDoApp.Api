using Microsoft.EntityFrameworkCore;
using TodoAPP.Core.DTOs;
using TodoAPP.Core.Models;
using TodoAPP.Core.Repositories;
using TodoAPP.Core.Services;
using TodoAPP.Core.UnitOfWorks;

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


    public string CalculateHash(string password, string salt)
    {
        return PasswordHasher.ComputeHash(password, salt, _pepper, _iteration);
    }
   
    public async Task<string> Register(UserDto userDto,UserInfoDetailed userInfoDetailed)
    {
        /*
           1- Checks if user exist
           2- If user exist return error message
           3- If user not exist creates new user returns empty string
           4- Return success message
        
         */
        
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
            newUser.PasswordHash= CalculateHash(userDto.Password, newUser.PasswordSalt);
        
            await _userInfoRepository.AddAsync(newUser);
            await _unitOfWork.CommitAsync();
            return string.Empty;
        }
        
        return errorMessage;

    }
    
    public async Task<string> Login(UserDto userDto)
    {
        /*
           1- Tries to get user by email
           2- If user already logged in return error message
           3- If user not logged in checks password
           4- If password is wrong return error message
           5- If password is correct return empty string
         */
        var userInfoWithDetails = await _userInfoRepository.GetUserInfoWithDetailsByEmail(userDto.Email);

        if (userInfoWithDetails.UserInfoDetailed.SessionGUID !=string.Empty)
        {
            return "User already logged in";
        }

        var passwordHash = PasswordHasher.ComputeHash(userDto.Password, userInfoWithDetails!.PasswordSalt, _pepper, _iteration);

        if (passwordHash != userInfoWithDetails.PasswordHash)
        {
            return "Please check your credentials";
        }


        return string.Empty;

    }

    public async Task<string> CheckUserExist(UserDto userDto)
    {
        /*
           1- Checks if user exist by email or username
           2- If user exist return error message
           3- If user not exist return empty string
         */
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
        // Sets user isDeleted property to true
        if (userInfo != null)
        {
            userInfo.IsDeleted = true;
            await Update(userInfo);
        }

        return userInfo;    
    }


    
   

}