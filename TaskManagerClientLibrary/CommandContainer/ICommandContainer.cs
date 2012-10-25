using System.Collections.Generic;
using TaskManagerClientLibrary.ConcreteCommands;

namespace TaskManagerClientLibrary.CommandContainer
{
    public interface ICommandContainer
    {
        IEnumerable<ICommand> GetCommands();
    }
}