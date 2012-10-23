using EntitiesLibrary;

namespace Specifications.ServiceSpecifications
{
    public class ListSingleServiceSpecification : IServiceSpecification
    {
        public object Data { get; set; }

        public bool IsSatisfied(ServiceTask task)
        {
            return (int)Data - 1 == task.Id;
        }
    }
}