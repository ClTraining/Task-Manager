using System;
using System.Collections.Generic;
using EntitiesLibrary;
using FluentAssertions;
using NSubstitute;
using Ninject;
using Ninject.Modules;
using TaskManagerHost.DataBaseAccessLayer;
using TaskManagerHost.TaskManager;
using Xunit;

namespace TaskManagerHost.WCFServer
{
    public class TaskManagerService : ITaskManagerService
    {
        public IToDoList TaskList;

        public TaskManagerService()
        {
            TaskList = new StandardKernel(new TaskManagerModule()).Get<ToDoList>();

            Console.WriteLine("new request added");
        }

        public int AddTask(string task)
        {
            return TaskList.AddTask(task);
        }

        public ContractTask GetTaskById(int id)
        {
            return TaskList.GetTaskById(id);
        }

        public List<ContractTask> GetAllTasks()
        {
            return TaskList.GetAllTasks();
        }

        public bool MarkCompleted(int id)
        {
            return TaskList.MarkCompleted(id);
        }

        public bool TestConnection()
        {
            return true;
        }
    }
    
    public class TaskManagerModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IRepository>().To<MemoRepository>().InSingletonScope();
            Bind<ITaskFactory>().To<TaskFactory>();
            Bind<IToDoList>().To<ToDoList>().InSingletonScope();
            Bind<ITaskMapper>().To<TaskMapper>();
        }
    }

    public class TaskManagerServiceTests
    {
        readonly TaskManagerService service = new TaskManagerService{TaskList = Substitute.For<IToDoList>()};
        readonly List<ContractTask> list = new List<ContractTask>{new ContractTask { Name = "some", Id = 1 }};

        [Fact]
        public void should_create_task_and_return_taskid()
        {
            const string taskName = "some id";
            service.TaskList.AddTask(taskName).Returns(1);
            var res = service.AddTask(taskName);
            res.Should().Be(1);
        }
        [Fact]
        public void should_get_task_by_id_and_return_task()
        {
            service.TaskList.GetTaskById(1).Returns(list[0]);
            var res = service.GetTaskById(1);
            res.Should().Be(list[0]);
        }
        [Fact]
        public void should_get_all_taasks()
        {
            service.TaskList.GetAllTasks().Returns(list);
            var res = service.GetAllTasks();
            res.Should().BeEquivalentTo(list);
        }
    }
}