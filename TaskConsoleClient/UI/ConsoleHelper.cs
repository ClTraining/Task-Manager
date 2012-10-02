using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using EntitiesLibrary;
using FluentAssertions;
using TaskConsoleClient.ConreteHandlers;
using TaskConsoleClient.Manager;
using NSubstitute;
using TaskManagerHost.WCFServer;
using Xunit;

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
