using System;
using System.Collections.Generic;
using ConnectToWcf;
using EntitiesLibrary;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace TaskManagerConsole.ConcreteHandlers
{
    public class List : Command<string>
    {
        private readonly IClientConnection manager;

        public List(IClientConnection manager)
        {
            this.manager = manager;
            var fullName = this.ToString();
            Name = fullName.Substring(fullName.LastIndexOf('.') + 1).ToLower();
        }

        protected override void Execute(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                manager
                    .GetAllTasks()
                    .ForEach(x =>
                        Console.WriteLine("ID: {0}\tTask: {1}\tCompleted: {2}", x.Id, x.Name, x.IsCompleted ? "+" : "-"));
            }
            else
            {
                var argument = int.Parse(input);
                var task = manager.GetTaskById(argument);
                Console.WriteLine("ID: {0}\tTask: {1}\tCompleted: {2}", task.Id, task.Name, task.IsCompleted ? "+" : "-");
            }
        }
    }

    public class ListTester
    {
        private readonly List list = new List(new ClientConnection());

        [Fact]
        public void list_name_should_be_the_same_as_class_name()
        {
            list.Name.Should().BeEquivalentTo("list");
        }
    }
}
