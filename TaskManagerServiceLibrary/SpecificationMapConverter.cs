using System.Linq;
using AutoMapper;

namespace TaskManagerServiceLibrary
{
    public class SpecificationMapConverter<TSource, TDestination> : ITypeConverter<TSource, TDestination>
        where TSource : class
        where TDestination : class
    {
        public TDestination Convert(ResolutionContext context)
        {
            var baseType = context.SourceValue.GetType();

            var map = (from maps in Mapper.GetAllTypeMaps()
                                  where maps.SourceType == baseType
                                  select maps).FirstOrDefault();

            if (map != null)
            {
                var destType = map.DestinationType;
                return Mapper.DynamicMap(context.SourceValue, baseType, destType) as TDestination;
            }

            return null;
        }
    }
}
