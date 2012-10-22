using EntitiesLibrary;

namespace Specifications.ServiceSpecifications
{
    public class ListAllServiceSpecification : IServiceSpecification
    {
        public object Data { get; set; }

        public bool IsSatisfied(ServiceTask task)
        {
            return true;
        }
    }
}
