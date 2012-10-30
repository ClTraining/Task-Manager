using System;

namespace CommandQueryLibrary.ClientSpecifications
{
    public class ListByDateClientSpecification : IClientSpecification
    {
        public DateTime Date { get; set; }
    }
}
