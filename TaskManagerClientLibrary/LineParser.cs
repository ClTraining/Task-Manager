using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using ConnectToWcf;
using FluentAssertions;
using NSubstitute;
using TaskManagerClientLibrary.ConcreteHandlers;
using Xunit;

namespace TaskManagerClientLibrary
{
    public class LineParser
    {
        private readonly List<ICommand> commands;

        public LineParser(List<ICommand> commands)
        {
            this.commands = commands;
        }

        public List<string> GetArguments(string input)
        {
            var args = input.Split(' ').ToList();

            return new List<string> { args[0], args.Count > 1 ? string.Join(" ", args.Skip(1)) : string.Empty };
        }
        public void ExecuteCommand(string input)
        {
            var args = GetArguments(input);
            try
            {
                commands.First(a => a.Name == args[0]).Execute(args[1]);
            }
            catch (FaultException<ExceptionDetail> e)
            {
                Console.WriteLine("Task not found: (Id = {0})", e.Detail.Message);
            }
            catch (InvalidOperationException)
            {
                Console.WriteLine("This command is incorrect. Please, try again!");
            }
        }
    }

    public class LineParserTester
    {
        private readonly IClientConnection client = Substitute.For<IClientConnection>();
        private readonly LineParser lp;

        public LineParserTester()
        {
            lp = new LineParser(new List<ICommand> { new List(client) });
        }


        [Fact]
        public void should_split_the_input_on_command_and_argument()
        {
            var command = lp.GetArguments("hello world");

            command.Should().ContainInOrder(new[] { "hello", "world" });
        }
    }
}
