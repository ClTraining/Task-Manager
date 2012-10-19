using EntitiesLibrary;

namespace TaskManagerServiceLibrary.Specifications
{
    public class ListSingleSpecification : ISpecification
    {
        private readonly int id;

        public ListSingleSpecification(int id)
        {
            this.id = id;
        }

        public bool IsSatisfied(ServiceTask task)
        {
            return task.Id == id;
        }
    }
}