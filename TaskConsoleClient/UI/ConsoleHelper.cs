using System;
using System.Collections.Generic;
using System.Linq;
using TaskConsoleClient.ConcreteHandlers;
using TaskManagerHost.WCFServer;

namespace TaskConsoleClient.UI
{
    internal class ConsoleHelper
    { 
        private readonly List<ICommandHandler> handler;

        public ConsoleHelper(List<ICommandHandler> handler)
        {
            this.handler = handler;
        }

        public void Execute(string input)
        {
            try
            {
                handler.First(x => x.Matches(input)).Execute();
            }
            catch (TaskNotFoundException e)
            {
                Console.WriteLine("Task not found. Task ID: {0}", e.TaskId);
            }
            catch(InvalidOperationException e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
