using System;
using EntitiesLibrary;

namespace Specifications.ServiceSpecifications
{
    public class ListByDateSpecification:IServiceSpecification
    {
        public DateTime Date { get; set; }

        public bool IsSatisfied(ServiceTask task)
        {
            return false;
        }
    }
}
