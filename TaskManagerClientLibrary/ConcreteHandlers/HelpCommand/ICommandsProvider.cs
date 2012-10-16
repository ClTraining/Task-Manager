using System.Collections.Generic;

namespace TaskManagerClientLibrary.ConcreteHandlers.HelpCommand
{
    public interface ICommandsProvider
    {
        IEnumerable<ICommand> Commands { get; set; }
    }

    public class CommandsProvider : ICommandsProvider
    {
        public IEnumerable<ICommand> Commands { get; set; }

        public CommandsProvider(IEnumerable<ICommand> commands)
        {
            Commands = commands;
        }
    }
}
