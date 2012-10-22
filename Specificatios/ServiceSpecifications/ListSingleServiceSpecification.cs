using EntitiesLibrary;

namespace Specifications.ServiceSpecifications
{
    public class ListSingleServiceSpecification : IServiceSpecification
    {
        private readonly int id;

        public ListSingleServiceSpecification() {}

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