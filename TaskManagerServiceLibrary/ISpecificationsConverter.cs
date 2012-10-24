using System.Collections.Generic;
using Specifications.ClientSpecification;
using Specifications.QuerySpecifications;

namespace TaskManagerServiceLibrary
{
    public interface ISpecificationsConverter
    {
        IQuerySpecification GetQuerySpecification(IClientSpecification specification);
    }
}
