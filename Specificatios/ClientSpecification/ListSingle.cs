using System.Collections;
using System.Runtime.Serialization;

namespace Specifications.ClientSpecification
{
    [DataContract]
    public class ListSingle : IClientSpecification
    {
        [DataMember]
        public int Id { get; set; }
    }
}
