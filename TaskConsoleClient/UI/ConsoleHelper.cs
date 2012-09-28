using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using TaskConsoleClient.Manager;
using NSubstitute;
using Xunit;

namespace TaskConsoleClient.UI
{
    class ConsoleHelper
    {
        private readonly ICommandManager commandManager;

        public ConsoleHelper(ICommandManager commandManager)
        {
            this.commandManager = commandManager;
        }

        public void ExecuteCommand(string text)
        {
            try
            {
                if (!IsContainsCommands(text))
                    throw new InvalidCommandException("Command is not supported");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            var command = GetCommand(text);
            switch (command)
            {
                case "add ":
                    Console.WriteLine("Task added. Task ID: " + commandManager.AddTask(text.Substring(4)));
                    break;

                case "list":
                    commandManager.GetAllTasks().ForEach(Console.WriteLine);
                    break;

                case "list ":
                    {
                        try
                        {
                            var id = int.Parse(text.Substring(5));
                            var task = commandManager.GetTaskById(id);

                            if (task == null) throw new NullReferenceException(string.Format("Task not found. Task ID"));

                            Console.WriteLine("ID: {0}\tTask: {1}", task.Id, task.Name);
                        }
                        catch (NullReferenceException e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }
                    break;
            }

        }

        public bool IsContainsCommands(string text)
        {
            var commands = new List<string> { "add ", "list ", "list" };
            return commands.Any(text.Contains);
        }

        public string GetCommand(string text)
        {
            if (text == "list")
                return text;

            var end = text.IndexOf(' ');
            var result = text.Substring(0, end + 1);
            return result;
        }
    }

    public class InvalidCommandException : Exception
    {
        public InvalidCommandException() { }

        public InvalidCommandException(string message)
            : base(message) { }
    }

    public class ConsoleHelperTests
    {
        private readonly ICommandManager cm = Substitute.For<ICommandManager>();
        private readonly ConsoleHelper consoleHelper;

        public ConsoleHelperTests()
        {
            consoleHelper = new ConsoleHelper(cm);    
        }

        [Fact]
        public void should_get_task_from_console()
        {
            // arrange
            var sh = new ConsoleHelper(cm);

            // act
            sh.ExecuteCommand("add hello world");

            // assert;
            cm.Received().AddTask("hello world");
        }

        [Fact]
        public void parse_when_passed_wrong_argument_should_throw_WrongArgumentException()
        {
            //arrange
            const string command = "asdasdasdasd";
            Action act = () => consoleHelper.ExecuteCommand(command);

            //act
            var res = consoleHelper.IsContainsCommands(command);

            //assert
            if (res) act.ShouldThrow<InvalidCommandException>().WithMessage("Command not supported");
        }

        [Fact]
        public void should_recognise_list_id_command()
        {
            // act
            consoleHelper.ExecuteCommand("list 5");

            // assert
            cm.Received().GetTaskById(5);
        }


        [Fact]
        public void should_recognise_add_command()
        {
            // act
            consoleHelper.ExecuteCommand("add Test task");

            // assert
            cm.Received().AddTask("Test task");
        }
    }
}
