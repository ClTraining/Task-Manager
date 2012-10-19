using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntitiesLibrary;

namespace TaskManagerServiceLibrary.Specifications
{
    public class ListAllSpecification : ISpecification
    {
        public bool IsSatisfied(ServiceTask task)
        {
            return task.Id > 0;
        }
    }
}
