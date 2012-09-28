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
        public IToDoList taskList;

        public TaskManagerService()
        {
            taskList = new StandardKernel(new TaskManagerModule()).Get<ToDoList>();

            Console.WriteLine("new request added");
        }

        public int AddTask(string task)
        {
            //return taskList.AddTask(task);
            return 1;
        }

        public ContractTask GetTaskById(int id)
        {
            return taskList.GetTaskById(id);
        }

        public List<ContractTask> GetAllTasks()
        {
            return taskList.GetAllTasks();
        }

        public bool MarkCompleted(int task)
        {
            throw new NotImplementedException();
        }

        public ContractTask MarkCompleted(ContractTask task)
        {
            return null;
        }
    }
    
    public class TaskManagerModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IRepository>().To<MemoRepository>();
            Bind<ITaskFactory>().To<TaskFactory>();
            Bind<IToDoList>().To<ToDoList>().InSingletonScope();
            Bind<ITaskMapper>().To<TaskMapper>();
        }
    }

    public class TaskManagerServiceTests
    {
        readonly TaskManagerService service = new TaskManagerService{taskList = Substitute.For<IToDoList>()};
        readonly ContractTask outTask = new ContractTask();
        readonly ContractTask inTask = new ContractTask{Id = 1};
        List<ContractTask> list = new List<ContractTask>();
        
        //[Fact]
        //public void should_create_task_and_return_taskid()
        //{
        //    service.taskList.AddTask(outTask).Returns(inTask);
        //    var res = service.AddTask(outTask);
        //    res.Should().Be(inTask);
        //}
        //[Fact]
        //public void should_get_task_by_id_and_return_task()
        //{
        //    service.taskList.GetTaskById(1).Returns(inTask);
        //    var res = service.GetTaskById(1);
        //    res.Should().Be(inTask);
        //}
        //[Fact]
        //public void should_get_all_taasks()
        //{
        //    list.Add(outTask);
        //    list.Add(inTask);
        //    service.taskList.GetAllTasks().Returns(list);
        //    var res = service.GetAllTasks();
        //    res.Should().BeEquivalentTo(list);
        //}
    }
}