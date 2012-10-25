using System.Collections.Generic;

namespace TaskManagerClientLibrary.ConcreteHandlers
{
    public interface ICommand
    {
        string Name { get; set; }
        string Description { get; set; }
        void Execute(List<string> argument);
    }
}