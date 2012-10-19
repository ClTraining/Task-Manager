using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntitiesLibrary.SpecificationContract
{
    public class ListSingle : ISpecificationContract
    {
        public int ID { get; private set; }

        public ListSingle(int id)
        {
            ID = id;
        }
    }
}
