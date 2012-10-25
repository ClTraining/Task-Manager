using System.Collections.Generic;

namespace TaskManagerClientLibrary.ConcreteHandlers
{
    public interface ICommand
    {
        string Name { get; }
        string Description { get; }
        void Execute(List<string> argument);
    }
}