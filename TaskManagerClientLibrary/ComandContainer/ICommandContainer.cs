using System.Collections.Generic;
using TaskManagerClientLibrary.ConcreteHandlers;

namespace TaskManagerClientLibrary.ComandContainer
{
    public interface ICommandContainer
    {
        IEnumerable<ICommand> GetCommands();
    }
}