using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using FluentAssertions;
using NSubstitute;
using TaskConsoleClient.ConcreteHandlers;
using TaskManagerHost.WCFServer;
using Xunit;

namespace TaskConsoleClient.UI
{
    public class ConsoleHelper
    {
        private readonly IEnumerable<ICommandHandler> commandHandlers;

        public ConsoleHelper(IEnumerable<ICommandHandler> commandHandlers)
        {
            this.commandHandlers = commandHandlers;
        }

        public void Execute(string command)
        {
            try
            {
                commandHandlers.First(x => x.Matches(command)).Execute(command);
            }
            catch (FaultException<ExceptionDetail> e)
            {
                Console.WriteLine("Task not found: (Id = {0})", e.Detail.Message);
            }
        }
    }

    public class ConsoleHelperTests
    {
        [Fact]
        public void should_find_matching_handler_and_execute_id()
        {
            var matching = Substitute.For<ICommandHandler>();
            var notMatching = Substitute.For<ICommandHandler>();
            var consoleHelper = new ConsoleHelper(new List<ICommandHandler> { notMatching, matching });

            const string command = "command";
            matching.Matches(command).Returns(true);

            consoleHelper.Execute(command);

            matching.Received().Execute(command);
        }

        [Fact]
        public void should_throw_exception_if_id_not_found()
        {
            var handler = Substitute.For<ICommandHandler>();
            const string command = "list 100";
            handler.Matches(command).Returns(true);
            var helper = new ConsoleHelper(new List<ICommandHandler> { handler });

            var faultException = new FaultException<ExceptionDetail>(new ExceptionDetail(new TaskNotFoundException(100)));

            handler
                .When(h => h.Execute(command))
                .Do(x => { throw faultException; });


            var sb = new StringBuilder();
            Console.SetOut(new StringWriter(sb));

            helper.Execute(command);

            sb.ToString().Should().Be("Task not found: (Id = 100)\r\n");
        }
    }


}
