using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntitiesLibrary.SpecificationContract
{
    public class ListByDate : ISpecificationContract
    {
        private DateTime date;

        public ListByDate(DateTime date)
        {
            this.date = date;
        }
    }
}
