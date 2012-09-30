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
                    var resultId = commandManager.AddTask(text.Substring(4));
                    Console.WriteLine("Task added. Task ID: " + resultId);
                    break;

                case "list":
                    commandManager
                        .GetAllTasks()
                        .ForEach(x => Console.WriteLine("ID: {0}\tTask: {1}\tCompleted: {2}", x.Id, x.Name, x.IsCompleted ? "+" : "-"));
                    break;

                case "list ":
                    try
                    {
                        var lid = int.Parse(text.Substring(command.Length));
                        var task = commandManager.GetTaskById(lid);

                        if (task == null) throw new NullReferenceException(string.Format("Task not found. Task ID"));
                        Console.WriteLine("ID: {0}\tTask: {1}\tCompleted: {2}", task.Id, task.Name, task.IsCompleted ? "+" : "-");
                    }
                    catch (NullReferenceException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    break;

                case "completed ":

                    var cid = int.Parse(text.Substring(command.Length));
                    commandManager.MarkCompleted(cid);
                    Console.WriteLine("Task ID: {0} completed", cid);
                    
                    break;
            }
        }

        public bool IsContainsCommands(string text)
        {
            var commands = new List<string> { "add ", "list ", "list", "completed " };
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

    public class InvalidCommandException : Exception
    {
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
            // act
            consoleHelper.ExecuteCommand("add hello world");

            // assert;
            cm.Received().AddTask("hello world");
        }

        [Fact]
        public void parse_when_passed_wrong_argument_should_throw_wrongargumentexception()
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

        [Fact]
        public void should_recognise_iscomplited_command()
        {
            // act
            consoleHelper.ExecuteCommand("completed 1");

            // assert
            cm.Received().MarkCompleted(1);
        }
    }
}
