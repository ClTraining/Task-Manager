using System;
using System.Collections.Generic;
using System.ServiceModel;
using EntitiesLibrary;
using TaskConsoleClient.UI;
using TaskManagerHost.WCFServer;

namespace TaskConsoleClient.Manager
{
    class CommandManager: ICommandManager
    {
        private ITaskManagerService client;

        public CommandManager()
        {
        }

        public ContractTask AddTask(ContractTask task)
        {
            throw new NotImplementedException();
        }

        public ContractTask GetTaskById(int id)
        {
            throw new NotImplementedException();
        }

        public List<ContractTask> GetAllTasks()
        {
            throw new NotImplementedException();
        }

        public ContractTask Edit(ContractTask task)
        {
            throw new NotImplementedException();
        }

    }
}

