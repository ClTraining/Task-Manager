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
        private readonly ICommandManager commandManager;

        public ConsoleHelper(ICommandManager commandManager)
        {
            this.commandManager = commandManager;
        }

        public void View(ContractTask task)
        {
            Console.WriteLine("Task ID: {0}\tTask Name: {1}", task.Id, task.Name);
        }

        public void Parse(string text)
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
                    var addedtask = commandManager.AddTask(new ContractTask {Name = text.Substring(4)});
                    View(addedtask);
                    break;
                case "list":
                    {
                        var tasks = commandManager.GetAllTasks();
                        foreach (var contractTask in tasks)
                            View(contractTask);
                    }
                    break;
                case "list ":
                    {
                        var task = commandManager.GetTaskById(int.Parse(text.Substring(5)));
                        View(task);
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
        //public void should_get_task_from_console()
        //{
        //    // arrange
        //    var sh = new ConsoleHelper(cm);

        //    // act
        //    var taskName = sh.Parse("add hello world");

        //    // assert
        //    taskName.Name.Should().BeEquivalentTo("hello world");
        //}

        [Fact]
        public void parse_when_passed_wrong_argument_should_throw_WrongArgumentException()
        {
            // arrange
            var consoleHelper = new ConsoleHelper(cm);
            // act
            Action action = () => consoleHelper.Parse("abrakadabra");

            action.ShouldThrow<WrongArgumentException>().WithInnerMessage("Command is not supported");
        }

        [Fact]
        public void view_should_show_on_console_the_info_in_task()
        {
            // arrange
            var expected = new StringBuilder();

            var consoleHelper = new ConsoleHelper(cm);
            var contractTask = new ContractTask { Name = "Buy Milk", Id = 1 };

            // act
            Console.SetOut(new StringWriter(expected));
            consoleHelper.View(contractTask);

            // assert
            expected.ToString().Should().BeEquivalentTo("Task ID: 1\tTask Name: Buy Milk\r\n");
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
            consoleHelper.Parse("list 5");

            // assert
            sb.ToString().Should().BeEquivalentTo("Task ID: 5\tTask Name: Test task\r\n");
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
            coMan.AddTask(null).ReturnsForAnyArgs(new ContractTask { Name = "Say Hello" });
            consoleHelper.Parse("add Test task");

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
            consoleHelper.Parse("list");

            sb.ToString().Should().BeEquivalentTo("Task ID: 1\tTask Name: Sasha\r\nTask ID: 2\tTask Name: Pasha\r\nTask ID: 3\tTask Name: Lena\r\n");
        }
    }
}
