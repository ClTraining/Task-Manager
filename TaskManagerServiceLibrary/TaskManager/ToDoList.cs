using System;
using System.Collections.Generic;
using System.Linq;
using EntitiesLibrary;
using EntitiesLibrary.CommandArguments;
using FluentAssertions;
using NSubstitute;
using TaskManagerServiceLibrary.Repositories;
using Xunit;

namespace TaskManagerServiceLibrary.TaskManager
{
    public class ToDoList : IToDoList
    {
        private readonly ITaskMapper mapper;
        private readonly IRepository repository;

        public ToDoList(IRepository repository, ITaskMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        #region IToDoList Members

        public int AddTask(AddTaskArgs name)
        {
            return repository.AddTask(name);
        }

        public ContractTask GetTaskById(int id)
        {
            return mapper.ConvertToContract(repository.GetTaskById(id));
        }

        public List<ContractTask> GetAllTasks()
        {
            return repository.GetAllTasks()
                .Select(mapper.ConvertToContract)
                .ToList();
        }

        public void MarkTaskAsCompleted(CompleteTaskArgs id)
        {
            repository.MarkTaskAsCompleted(id);
        }

        public void RenameTask(RenameTaskArgs args)
        {
            repository.RenameTask(args);
        }

        public void SetTaskDueDate(SetDateArgs args)
        {
            repository.SetTaskDueDate(args);
        }

        #endregion
    }


    public class ToDoListTests
    {
        private readonly ITaskMapper mapper = Substitute.For<ITaskMapper>();
        private readonly IRepository repository = Substitute.For<IRepository>();
        private readonly IToDoList todolist;

        public ToDoListTests()
        {
            todolist = new ToDoList(repository, mapper);
        }

        [Fact]
        public void should_save_task_and_generate_new_id()
        {
            var addTaskArgs = new AddTaskArgs {Name = "new task"};
            repository.AddTask(addTaskArgs).Returns(1);
            var newtask = todolist.AddTask(addTaskArgs);
            newtask.Should().Be(1);
        }

        [Fact]
        public void should_get_task_by_id()
        {
            var serviceTask = new ServiceTask();
            var contractTask = new ContractTask();
            repository.GetTaskById(1).Returns(serviceTask);
            mapper.ConvertToContract(serviceTask).Returns(contractTask);

            var res = todolist.GetTaskById(1);
            res.Should().Be(contractTask);
        }

        [Fact]
        public void should_get_all_tasks()
        {
            var serviceTask = new ServiceTask {Id = 1, Name = "some"};
            var serviceTasks = new List<ServiceTask> {serviceTask};
            var contractTask = new ContractTask {Id = 1, Name = "some"};
            var contractTasks = new List<ContractTask> {contractTask};

            repository.GetAllTasks().Returns(serviceTasks);
            mapper.ConvertToContract(serviceTask).Returns(contractTask);

            var res = todolist.GetAllTasks();
            res.Should().BeEquivalentTo(contractTasks);
        }

        [Fact]
        public void should_mark_task_as_completed_by_id()
        {
            var completeTaskArgs = new CompleteTaskArgs {Id = 1};
            todolist.MarkTaskAsCompleted(completeTaskArgs);
            repository.Received().MarkTaskAsCompleted(completeTaskArgs);
        }

        [Fact]
        public void should_send_rename_task_to_repository()
        {
            var args = new RenameTaskArgs {Id = 1, Name = "task name"};
            todolist.RenameTask(args);
            repository.Received().RenameTask(args);
        }

        [Fact]
        public void should_send_set_date_for_task_to_repository()
        {
            var args = new SetDateArgs {Id = 1, DueDate = DateTime.Now};
            todolist.SetTaskDueDate(args);
            repository.Received().SetTaskDueDate(args);
        }
    }
}