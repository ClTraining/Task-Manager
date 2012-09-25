#region Using

using System;
using TaskConsoleClient.Entities;
using TaskConsoleClient.WCFClient;

#endregion


namespace TaskConsoleClient.Manager
{
    class CommandManager: ICommandManager
    {
        public TaskContract AddTask(TaskContract task)
        {
            throw new NotImplementedException();
        }
    }
}
