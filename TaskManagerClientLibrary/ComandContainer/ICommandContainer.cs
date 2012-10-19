using System.Collections.Generic;
using TaskManagerClientLibrary.ConcreteHandlers;

namespace TaskManagerClientLibrary
{
    public interface ICommandContainer
    {
        IEnumerable<ICommand> GetCommands();
    }
}
