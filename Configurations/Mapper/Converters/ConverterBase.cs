using System;

namespace AirIQ.Configurations.Mapper.Converters;

public abstract class ConverterBase<TSource, TDestination> : IConverter<TSource, TDestination>
        where TSource : class
        where TDestination : class
{
    public TDestination Convert(TSource source)
    {
        if (source is null)
        {
            return null;
        }

        return ConvertImpl(source);
    }

    protected abstract TDestination ConvertImpl(TSource source);
}
