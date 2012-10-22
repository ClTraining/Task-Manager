using EntitiesLibrary;
using Specifications.ClientSpecification;

namespace Specifications.ServiceSpecifications
{
    public class ListAllServiceSpecification : IServiceSpecification
    {
        public bool IsSatisfied(ServiceTask task)
        {
            return true;
        }
    }
}
