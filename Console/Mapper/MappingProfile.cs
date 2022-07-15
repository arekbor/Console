using AutoMapper;
using Console.ModelDto;
using Console.Models;

namespace Console.Mapper;

public class MappingProfile:Profile
{
    public MappingProfile()
    {
        CreateMap<User, RegisterUserDto>()
            .ReverseMap();

        CreateMap<User, LoginUserDto>()
            .ReverseMap();
        
        CreateMap<User, UserDto>()
            .ReverseMap();
    }
}