using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using ConnectToWcf;
using FluentAssertions;
using TaskManagerConsole.ConcreteHandlers;
using Xunit;
using NSubstitute;

namespace TaskManagerConsole
{
    public class LineParser
    {
        private readonly List<ICommandHandler> comands;

        public LineParser(List<ICommandHandler> comands)
        {
            this.comands = comands;
        }

        public List<string> SplitInput(string input)
        {
            var result = input.Split(' ').ToList();
            result.Add(string.Empty);
            return result;
        }

        public void ExecuteComand(string input)
        {
            try
            {
                var args = SplitInput(input);
                var comand = comands.FirstOrDefault(x => x.Name == args[0]);
                if (comand != null)
                {
                    var argument = comand.Convert(args[1]);
                    comand.Execute(argument);
                }
                else
                    Console.WriteLine("This comand is incorrect. Please, try again!");
            }
            catch (FaultException<ExceptionDetail> e)
            {
                Console.WriteLine("Task not found: (Id = {0})", e.Detail.Message);
            }
        }
    }

    public class LineParserTester
    {
        private readonly LineParser lp = new LineParser(new List<ICommandHandler> { new List(new ClientConnection()) });
        private readonly ICommandHandler cEx = Substitute.For<ICommandHandler>();


        [Fact]
        public void should_split_the_input_on_command_and_argument()
        {
            var command = lp.SplitInput("hello world");

            command.Should().ContainInOrder(new[] { "hello", "world" });
        }

        [Fact]
        public void execute_command_should_call_proper_comand()
        {
            lp.ExecuteComand("add liliki");

            cEx.Received();
        }
    }
}
