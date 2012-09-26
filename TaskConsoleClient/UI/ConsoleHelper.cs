using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using EntitiesLibrary;
using FluentAssertions;
using Xunit;


namespace TaskConsoleClient.UI
{
    class ConsoleHelper : IConsoleHelper
    {
        public void View(ContractTask task)
        {
            Console.WriteLine("Task ID: {0}\tTask Name: {1}", task.Id, task.Name);
        }

        public ContractTask Parse(string text)
        {
            ContractTask result = null;
            if (text.StartsWith("add "))
            {
                result = new ContractTask { Name = text.Substring(4) };
                Console.WriteLine("Created new Task: {0}", result.Name);
            }
            else
            {
                throw new WrongArgumentException("Command is not supported");
            }

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
        public void should_get_task_from_console()
        {
            // arrange
            var sh = new ConsoleHelper();

            // act
            var taskName = sh.Parse("add hello world");

            // assert
            taskName.Name.Should().BeEquivalentTo("hello world");
        }

        [Fact]
        public void when_passed_wrong_argument_should_throw_WrongArgumentException()
        {
            // arrange
            var consoleHelper = new ConsoleHelper();
            // act
            Action action = () => consoleHelper.Parse("abrakadabra");

            action.ShouldThrow<WrongArgumentException>().WithMessage("Command is not supported");
        }

        [Fact]
        public void should_show_on_console_the_info_in_task()
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
    }
}
