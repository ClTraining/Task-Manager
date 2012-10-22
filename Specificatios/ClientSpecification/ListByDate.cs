using System;

namespace Specifications.ClientSpecification
{
    public class ListByDate : IClientSpecification
    {
        public ListByDate(DateTime date)
        {
            Data = date;
        }

        public DateTime Data { get; set; }
    }
}
