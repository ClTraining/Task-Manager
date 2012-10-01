using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using EntitiesLibrary;
using FluentAssertions;
using TaskConsoleClient.Manager;
using NSubstitute;
using Xunit;

namespace TaskConsoleClient.UI
{
    class ConsoleHelper
    {
        private readonly List<string> commandPatterms = new List<string>
                                      {
                                          @"^(add)\s",
                                          @"^list$",
                                          @"^(list\s)\d+$",
                                          @"^(completed)\s\d+$"
                                      };

        private readonly ICommandManager commandManager;

        public ConsoleHelper(ICommandManager commandManager)
        {
            this.commandManager = commandManager;
        }

        public void ExecuteCommand(string text)
        {
            if (!IsCommandSupported(text)) return;
            var command = GetCommand(text);
            switch (command)
            {
                case "add":
                    AddTask(text);
                    break;
                case "list":
                    ListTasks();
                    break;
                case "list ":
                case "list\t":
                    ListSingleTask(text, command);
                    break;
                case "completed":
                    MarkTaskCompleted(text, command);
                    break;
            }
        }

        private void MarkTaskCompleted(string text, string command)
        {
            var cid = int.Parse(text.Substring(command.Length));
            commandManager.MarkCompleted(cid);
            Console.WriteLine("Task ID: {0} completed", cid);
        }

        private void ListSingleTask(string text, string command)
        {
            //try
            //{
            var lid = int.Parse(text.Substring(command.Length));
            var task = commandManager.GetTaskById(lid);

            if (task == null)
            {
                Console.WriteLine(string.Format("Task not found (ID: {0})", lid));
                return;
            }
            ViewTaskInfo(task);
        }

        private void ListTasks()
        {
            commandManager
                .GetAllTasks()
                .ForEach(x => { if (x != null) ViewTaskInfo(x); });
        }

        private void ViewTaskInfo(ContractTask task)
        {
            Console.WriteLine("ID: {0}\tTask: {1}\tCompleted: {2}", task.Id, task.Name, task.IsCompleted ? "+" : "-");
        }

        private void AddTask(string text)
        {
            var resultId = commandManager.AddTask(text.Substring(4));
            Console.WriteLine("Task added. Task ID: " + resultId);
        }

        private bool IsCommandSupported(string text)
        {
            var result = true;
            if (!IsCommandCorrect(text))
            {
                result = false;
                Console.WriteLine("Command is not supported");
            }

            return result;
        }

        private bool IsCommandCorrect(string text)
        {
            var regexes = commandPatterms.Select(x => new Regex(x)).ToList();
            return regexes.Any(regex => regex.IsMatch(text));
        }

        private string GetCommand(string text)
        {
            var regexes = commandPatterms.Select(x => new Regex(x)).ToList();
            var regex = regexes.FirstOrDefault(x => x.IsMatch(text));
            var match = regex.Match(text);
            Group group = null;
            if (match.Success)
                group = match.Groups[1];

            return group.ToString();
        }


        private void PrintExceptionInfo(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
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

        [Fact]
        public void test_regex_list_all()
        {
            var r = new Regex(@"^list$");
            var result = r.IsMatch("list");
            result.Should().Be(true);
        }

        [Fact]
        public void test_regex_add()
        {
            // arrange
            var r = new Regex(@"^add\s");
            var result = r.IsMatch("add\nlkhglkhglyglygl jykjyk");
            result.Should().Be(true);
        }

        [Fact]
        public void test_regex_list_task()
        {
            var r = new Regex(@"^list\s\d+$");
            var result = r.IsMatch("list 0");
            result.Should().Be(true);
        }

        [Fact]
        public void test_regex_completed_task()
        {
            var r = new Regex(@"^completed\s\d+$");
            var result = r.IsMatch("completed 4");
            result.Should().Be(true);
        }
    }
}
