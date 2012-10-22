using System.Collections;
using System.Runtime.Serialization;

namespace Specifications.ClientSpecification
{
    public class ListSingle : IClientSpecification
    {
        public ListSingle(int id)
        {
            Id = id;
        }
        
        public int Id { get; set; }
    }
}
