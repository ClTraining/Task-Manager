using System;

namespace EntitiesLibrary
{
    public class ContractTask
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsCompleted { get; set; }

        public DateTime DueDate { get; set; }
    }
}