using System.Collections.Generic;
using TaskManagerClientLibrary.ConcreteHandlers;

namespace TaskManagerClientLibrary.ComandContainer
{
    public interface ICommandContainer
    {
        List<ICommand> GetCommands();
    }
}