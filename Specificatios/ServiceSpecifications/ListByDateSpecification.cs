using System;
using EntitiesLibrary;

namespace Specifications.ServiceSpecifications
{
    public class ListByDateSpecification : IServiceSpecification
    {
        public object Data { get; set; }

        public bool IsSatisfied(ServiceTask task)
        {
            return (DateTime)Data == task.DueDate;
        }
    }
}
