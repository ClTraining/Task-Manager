using System;

namespace CQRS.ClientSpecifications
{
    public class ListByDateClientSpecification : IClientSpecification
    {
        public DateTime Date { get; set; }
    }
}
