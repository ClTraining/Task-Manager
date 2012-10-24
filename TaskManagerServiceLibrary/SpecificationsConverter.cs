using System.Collections.Generic;
using System.Linq;
using Specifications.ClientSpecification;
using Specifications.QuerySpecifications;

namespace TaskManagerServiceLibrary
{
    public class SpecificationsConverter : ISpecificationsConverter
    {
        private readonly IEnumerable<IQuerySpecification> serviceSpecifications;

        public SpecificationsConverter(IEnumerable<IQuerySpecification> serviceSpecifications)
        {
            this.serviceSpecifications = serviceSpecifications;
        }

        public IQuerySpecification GetQuerySpecification(IClientSpecification specification)
        {
            return serviceSpecifications.First(x => x.GetType().Name.Contains(specification.GetType().Name));
        }
    }
}