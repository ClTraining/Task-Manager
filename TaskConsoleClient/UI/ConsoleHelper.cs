using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EntitiesLibrary;
using FluentAssertions;
using TaskConsoleClient.Manager;
using Xunit;
using NSubstitute;


namespace TaskConsoleClient.UI
{
    class ConsoleHelper : IConsoleHelper
    {
        public void View(ContractTask task)
        {
            Console.WriteLine("Task ID: {0}\tTask Name: {1}", task.Id, task.Name);
        }

        //public ContractTask Parse(string text)
        //{
        //    //var commands = new List<string> { "add ", "list ", "list" };

        //    //var commandManager = new CommandManager();

        //    ContractTask result = null;
        //    if (text.StartsWith("add "))
        //    {
        //        result = new ContractTask { Name = text.Substring(4) };
        //        //commandManager.AddTask(result);
        //        Console.WriteLine("Created new Task: {0}", result.Name);
        //    }
        //    else
        //    {
        //        throw new WrongArgumentException("Command is not supported");
        //    }

        //    return result;
        //}
        public void Parse(string text)
        {
            var commandManager = new CommandManager();

            if (IsContainsCommands(text))
            {
                var command = GetCommand(text);
                switch (command)
                {
                    case "add ":
                        commandManager.AddTask(new ContractTask { Name = text.Substring(4) });
                        break;
                    case "list":
                        commandManager.ViewAllTasks();
                        break;
                    case "list ":
                        commandManager.ViewTaskById(int.Parse(text.Substring(5)));
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
            else
                throw new WrongArgumentException("Command is not supported");


        }

        private bool IsContainsCommands(string text)
        {
            var commands = new List<string> { "add ", "list ", "list" };
            return commands.Any(text.Contains);
        }

        private string GetCommand(string text)
        {
            var end = text.IndexOf(" ");
            var result = text.Substring(0, end + 1);
            Console.Out.WriteLine("end = {0}", result);
            return result;
        }
    }

    public class WrongArgumentException : Exception
    {
        public WrongArgumentException(string message)
            : base(message) { }
    }

    public class ConsoleHelperTester
    {

        [Fact]
        public void parse_when_passed_wrong_argument_should_throw_WrongArgumentException()
        {
            // arrange
            var consoleHelper = new ConsoleHelper();
            // act
            Action action = () => consoleHelper.Parse("abrakadabra");

            action.ShouldThrow<WrongArgumentException>().WithMessage("Command is not supported");
        }

        [Fact]
        public void view_should_show_on_console_the_info_in_task()
        {
            // arrange
            var expected = new StringBuilder();

            var consoleHelper = new ConsoleHelper();
            var contractTask = new ContractTask { Name = "Buy Milk", Id = 1 };

            // act
            Console.SetOut(new StringWriter(expected));
            consoleHelper.View(contractTask);

            // assert
            expected.ToString().Should().BeEquivalentTo("Task ID: 1\tTask Name: Buy Milk\r\n");
        }

        [Fact]
        public void parse_when_passed_list_command_should_show_task_by_id()
        {
            // arrange
            var consoleHelper = new ConsoleHelper();
            var cm = NSubstitute.Substitute.For<ICommandManager>();

            // act
            
        }
    }
}
