using AutoMapper;
using Specifications.ClientSpecification;
using Specifications.ServiceSpecifications;

namespace Specifications.Mappers
{
    public class ServiceMapper
    {
        readonly IClientSpecification sourceType;
        readonly IServiceSpecification destinationType;
        public ServiceMapper(IClientSpecification spec1, IServiceSpecification spec2)
        {
            sourceType = spec1;
            destinationType = spec2;
            Mapper.CreateMap(sourceType.GetType(), destinationType.GetType());
        }

        public IServiceSpecification ConvertToServiceSpec(IClientSpecification spec)
        {
            return null;
        }
    }
}
