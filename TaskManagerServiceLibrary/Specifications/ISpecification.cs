using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntitiesLibrary;

namespace TaskManagerServiceLibrary.Specifications
{
    public interface ISpecification
    {
        bool IsSatisfied(ServiceTask task);
    }
}
