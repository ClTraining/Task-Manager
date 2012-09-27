using System.Runtime.Serialization;

namespace EntitiesLibrary
{
    public class ContractTask
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool Completed { get; set; }
    }
}
