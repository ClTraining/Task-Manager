using EntitiesLibrary;
using Specifications.ClientSpecification;

namespace Specifications.ServiceSpecifications
{
    public class ListSingleServiceSpecification : IServiceSpecification
    {
        public int ID { get; set; }

        public bool IsSatisfied(ServiceTask task)
        {
            return ID == task.Id;
        }
    }
}