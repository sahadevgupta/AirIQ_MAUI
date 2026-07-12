using AirIQ.Models;
using AirIQ.Models.Response;

namespace AirIQ.Configurations.Mapper.Converters
{
    public class LookupItemDtoToLookupItemConverter : ConverterBase<LookupItemDto, LookupItem>
    {
        protected override LookupItem ConvertImpl(LookupItemDto source)
        {
            return new LookupItem
            {
                Id = source.Id,
                Name = source.Name ?? string.Empty
            };
        }
    }
}