using AutoMapper;

namespace Specifications.Mappers
{
    public class ServiceMapper<T1, T2>
    {
        public ServiceMapper()
        {
            Mapper.CreateMap<T1, T2>();
        }

        public T2 ConvertToServiceSpec(T1 spec)
        {
            return Mapper.Map<T1, T2>(spec);
        }
    }
}
