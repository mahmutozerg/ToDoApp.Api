using AutoMapper;
using TodoAPP.Core.DTOs;
using TodoAPP.Core.Models;

namespace TodoAPP.Service.Mapping;

public class MapProfile:Profile
{
    public MapProfile()
    {
        CreateMap<UserDto, UserInfo>().ReverseMap();
    }
}