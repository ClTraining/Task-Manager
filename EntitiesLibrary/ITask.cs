using System.Runtime.Serialization;
using System.ServiceModel;

namespace EntitiesLibrary
{
    [ServiceContract]
    public interface ITask
    {
        [DataMember]
        int Id { get; set; }
        [DataMember]
        string Name { get; set; }
    }
}