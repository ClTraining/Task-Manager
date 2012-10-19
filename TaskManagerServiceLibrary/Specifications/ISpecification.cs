using EntitiesLibrary;

namespace TaskManagerServiceLibrary.Specifications
{
    public interface ISpecification
    {
        bool IsSatisfied(ServiceTask task);
    }

    public abstract class BaseSpecification : ISpecification
    {
        public int? Id { get; set; }

        public  BaseSpecification() { }

        public BaseSpecification(int? id)
        {
            Id = id;
        }

        public abstract bool IsSatisfied(ServiceTask task);
    }
}
