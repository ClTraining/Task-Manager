using EntitiesLibrary;

namespace TaskManagerServiceLibrary.Specifications
{
    public class ListSingleSpecification : BaseSpecification
    {
        public override bool IsSatisfied(ServiceTask task)
        {
            return task.Id == Id;
        }
    }
}