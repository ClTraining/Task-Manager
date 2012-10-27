using EntitiesLibrary.CommandArguments;

namespace Specifications.ClientSpecifications
{
    public class ListSingleClientSpecification : IClientSpecification
    {
        public int Id { get; set; }
        public bool IsSatisfied(ListTaskArgs listArgs)
        {
            return listArgs.Id > 0;
        }
    }
}
