using System;
using AirIQ.Configurations.Mapper.Converters;
using AirIQ.Model;
using AirIQ.Model.Response;

namespace AirIQ.Configurations.Mapper;

public static class BackendToAppModelMapper
{
    public static User GetUser(UserDto? userDto)
    {
        if (userDto == null)
        {
            return new User();
        }
        var converter = new UserDtoToUseConverter();
        return converter.Convert(userDto);
    }
}
