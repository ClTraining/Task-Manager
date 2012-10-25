using System.Collections.Generic;

namespace TaskManagerClientLibrary.ConcreteCommands
{
    public interface ICommand
    {
        string Name { get; }
        string Description { get; }
        void Execute(List<string> argument);
    }
}