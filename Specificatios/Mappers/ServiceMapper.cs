using AutoMapper;
using Specifications.ClientSpecification;
using Specifications.QuerySpecifications;

namespace Specifications.Mappers
{
    public class ServiceMapper
    {
        readonly IClientSpecification sourceType;
        readonly IQuerySpecification destinationType;
        public ServiceMapper(IClientSpecification spec1, IQuerySpecification spec2)
        {
            sourceType = spec1;
            destinationType = spec2;
            Mapper.CreateMap(sourceType.GetType(), destinationType.GetType());
        }

        public IQuerySpecification ConvertToServiceSpec(IClientSpecification spec)
        {
            return null;
        }
    }
}
