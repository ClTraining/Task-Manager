using System;

namespace Specifications.ClientSpecification
{
    public class ListByDate : IClientSpecification
    {
        public DateTime Date { get; private set; }

        public ListByDate(DateTime date)
        {
            Date = date;
        }

    }
}
