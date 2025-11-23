using System;
using AirIQ.Models;
using AirIQ.Models.Response;

namespace AirIQ.Configurations.Mapper.Converters;

public class UserDtoToUseConverter : ConverterBase<UserDto, User>
{
    protected override User ConvertImpl(UserDto source)
    {
        return new User
        {
            AgencyId = source.AgencyId,
            AgencyName = source.AgencyName,
            Balance = source.Balance,
            City = source.City,
            ContactPerson = source.ContactPerson,
            Country = source.Country,
            EmailId = source.EmailId,
            MobileNumber = source.MobileNumber
        };
    }
}
