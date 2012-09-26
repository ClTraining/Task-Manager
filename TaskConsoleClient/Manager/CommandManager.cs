using System;
using System.Collections.Generic;
using EntitiesLibrary;
using TaskConsoleClient.UI;

namespace TaskConsoleClient.Manager
{
    class CommandManager : ICommandManager
    {
        private ConsoleHelper ch = new ConsoleHelper();

        public ContractTask AddTask(ContractTask task)
        {
            throw new NotImplementedException();
        }

        public void ViewTaskById(int id)
        {
            throw new NotImplementedException();
        }

        public void ViewAllTasks()
        {
            throw new NotImplementedException();
        }

        public ContractTask Edit(ContractTask task)
        {
            throw new NotImplementedException();
        }
        private ContractTask GetTaskById(int id)
        {
            return new ContractTask();
        }
    }
}
