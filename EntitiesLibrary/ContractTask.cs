using System.Runtime.Serialization;
using System.ServiceModel;

namespace EntitiesLibrary
{
    [DataContract]
    public class ContractTask : ITask
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }
    }
}
