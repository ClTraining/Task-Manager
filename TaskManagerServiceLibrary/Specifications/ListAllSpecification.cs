using EntitiesLibrary;

namespace TaskManagerServiceLibrary.Specifications
{
    public class ListAllSpecification : BaseSpecification
    {
        public override bool IsSatisfied(ServiceTask task)
        {
            return Id == null;
        }
    }
}
