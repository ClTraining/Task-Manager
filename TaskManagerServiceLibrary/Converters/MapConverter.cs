using System.Linq;
using AutoMapper;

namespace TaskManagerServiceLibrary.Converters
{
    public class MapConverter<TSource, TDestination> : ITypeConverter<TSource, TDestination>
        where TSource : class
        where TDestination : class
    {
        public TDestination Convert(ResolutionContext context)
        {
            var baseType = context.SourceValue.GetType();

            var map = Mapper.GetAllTypeMaps()
                .FirstOrDefault(x => x.SourceType == baseType);

            if (map != null)
            {
                var destType = map.DestinationType;
                return Mapper.DynamicMap(context.SourceValue, baseType, destType) as TDestination;
            }

            return null;
        }
    }
}