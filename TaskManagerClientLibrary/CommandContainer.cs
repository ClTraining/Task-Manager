using System.Collections.Generic;
using TaskManagerClientLibrary.ConcreteHandlers;

namespace TaskManagerClientLibrary
{
    public static class CommandContainer
    {
        public static IEnumerable<ICommand> Commands { get; set; }

        public static void SetCommands(IEnumerable<ICommand> commandList)
        {
            Commands = commandList;
        }
    }
}