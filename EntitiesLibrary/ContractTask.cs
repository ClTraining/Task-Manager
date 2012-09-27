﻿using System.Runtime.Serialization;


namespace EntitiesLibrary
{
    public class ContractTask
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        public bool Completed { get; set; }
    }
}
