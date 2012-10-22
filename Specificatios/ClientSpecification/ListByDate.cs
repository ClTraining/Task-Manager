using System;

namespace Specifications.ClientSpecification
{
    public class ListByDate : IClientSpecification
    {
        public ListByDate(DateTime date)
        {
            Data = date;
        }

        public object Data { get; set; }
    }
}
