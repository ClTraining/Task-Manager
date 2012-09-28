using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EntitiesLibrary;
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
                    throw new WrongArgumentException("Command is not supported");
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

        private bool IsContainsCommands(string text)
        {
            var commands = new List<string> { "add ", "list ", "list" };
            return commands.Any(text.Contains);
        }

        private string GetCommand(string text)
        {
            if (text == "list")
                return text;

            var end = text.IndexOf(' ');
            var result = text.Substring(0, end + 1);
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
        private readonly ICommandManager cm = Substitute.For<ICommandManager>();

        [Fact]
        public void should_get_task_from_console()
        {
            // arrange
            var sh = new ConsoleHelper(cm);
            cm.AddTask("add hello world").Returns(1);

            // act
            sh.ExecuteCommand("add hello world");

            // assert;
        }

        [Fact]
        public void parse_when_passed_wrong_argument_should_throw_WrongArgumentException()
        {
            // arrange
            var consoleHelper = new ConsoleHelper(cm);
            // act
            Action action = () => consoleHelper.ExecuteCommand("abrakadabra");

            action.ShouldThrow<WrongArgumentException>().WithMessage("Command is not supported");
        }

        [Fact]
        public void should_recognise_list_id_command()
        {
            // arrange
            var comMan = Substitute.For<ICommandManager>();
            var consoleHelper = new ConsoleHelper(comMan);
            var sb = new StringBuilder();
            Console.SetOut(new StringWriter(sb));

            // act
            comMan.GetTaskById(5).Returns(new ContractTask { Name = "Test task", Id = 5 });
            consoleHelper.ExecuteCommand("list 5");

            // assert
            sb.ToString().Should().Be("Task ID: 5\tTask Name: Test task\r\n");
        }


        [Fact]
        public void should_recognise_add_command()
        {
            // arrange
            var coMan = Substitute.For<ICommandManager>();
            var consoleHelper = new ConsoleHelper(coMan);
            var sb = new StringBuilder();
            Console.SetOut(new StringWriter(sb));

            // act
            coMan.AddTask(null).Returns(1);
            consoleHelper.ExecuteCommand("add Test task");

            // assert
            sb.ToString().Should().BeEquivalentTo("Task ID: 0\tTask Name: Say Hello\r\n");
        }

        [Fact]
        public void should_recognise_list_command()
        {
            // arrange
            var coMan = Substitute.For<ICommandManager>();
            var consoleHelper = new ConsoleHelper(coMan);
            var sb = new StringBuilder();
            Console.SetOut(new StringWriter(sb));

            //act
            coMan.GetAllTasks().Returns(new List<ContractTask>
                                            {
                                                new ContractTask{Name = "Sasha",Id=1},
                                                new ContractTask{Name = "Pasha",Id = 2},
                                                new ContractTask{Name = "Lena",Id=3}
                                            });
            consoleHelper.ExecuteCommand("list");

            sb.ToString().Should().BeEquivalentTo("Task ID: 1\tTask Name: Sasha\r\nTask ID: 2\tTask Name: Pasha\r\nTask ID: 3\tTask Name: Lena\r\n");
        }
    }
}
