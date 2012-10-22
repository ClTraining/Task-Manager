using EntitiesLibrary;
using Specifications.ClientSpecification;

namespace Specifications.ServiceSpecifications
{
    public class ListSingleServiceSpecification : IServiceSpecification
    {
        private int id;

        public ListSingleServiceSpecification(int id)
        {
            this.id = id;
        }

        public bool IsSatisfied(ServiceTask task)
        {
            return id == task.Id;
        }
    }
}