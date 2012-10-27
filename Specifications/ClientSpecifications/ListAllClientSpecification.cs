using EntitiesLibrary.CommandArguments;

namespace Specifications.ClientSpecifications
{
    public class ListAllClientSpecification : IClientSpecification 
    {
        public bool IsSatisfied(ListTaskArgs listArgs)
        {
            return true;
        }
    }
}
