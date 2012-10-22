using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
