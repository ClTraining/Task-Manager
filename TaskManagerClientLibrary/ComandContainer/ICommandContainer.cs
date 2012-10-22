using System.Collections.Generic;
using TaskManagerClientLibrary.ConcreteHandlers;

namespace TaskManagerClientLibrary
{
    public interface ICommandContainer
    {
        List<ICommand> GetCommands();
    }
}