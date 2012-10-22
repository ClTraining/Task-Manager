using System;
using System.Collections.Generic;
using System.ServiceModel;
using EntitiesLibrary;
using Ninject;
using Ninject.Extensions.Conventions;
using Ninject.Modules;
using Specifications.ClientSpecification;
using Specifications.ServiceSpecifications;
using TaskManagerServiceLibrary.Repositories;
using TaskManagerServiceLibrary.TaskManager;
using Xunit;
using System.Linq;

namespace TaskManagerServiceLibrary
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, IncludeExceptionDetailInFaults = true)]
    public class TaskManagerService : ITaskManagerService
    {
        private readonly IRepository repository;
        private readonly IToDoList taskList;
        private readonly List<IServiceSpecification> list;

        public TaskManagerService(IRepository repository, List<IServiceSpecification> list, IToDoList taskList)
        {
            this.repository = repository;
            this.list = list;
            this.taskList = taskList;
        }

        #region ITaskManagerService Members

        public int AddTask(AddTaskArgs task)
        {
            return taskList.AddTask(task);
        }

        public List<ContractTask> GetTasks(IClientSpecification specification)
        {
            var res = list.First(x => x.GetType().Name.Contains(specification.GetType().Name));
            res.Data = specification.Data;
            return repository.GetTasks(res);
        }
    }

        public void MarkTaskAsCompleted(CompleteTaskArgs args)
    {
            taskList.MarkTaskAsCompleted(args);
        public void should_get_tasks_from_repository()
        {
            List<IServiceSpecification> specs = new StandardKernel(new MyModule()).GetAll<IServiceSpecification>().ToList();

            object id = 3;
            var cSpec = new ListSingle(4);

        public void SetTaskDueDate(SetDateArgs args)
        {
            taskList.SetTaskDueDate(args);
        }

        #endregion
    
            var repo = new MemoRepository();
            var todolist = new ToDoList(repo);
        private readonly ITaskManagerService service;

            var service = new TaskManagerService(repo, specs, todolist);

            var tasks = new[] { "task1", "task2", "task3", "task4", "task5", "task6", "task7", "task8", "task9" };
            var addTaskArgs = new AddTaskArgs {Name = "some task"};
            list.AddTask(addTaskArgs).Returns(1);
            int res = service.AddTask(addTaskArgs);

            tasks.ToList().ForEach(x => service.AddTask(x));
            ContractTask res = service.GetTaskById(1);

            var result = service.GetTasks(cSpec);
            List<ContractTask> res = service.GetAllTasks();

            foreach (var task in result)
            {
            var completeTaskArgs = new CompleteTaskArgs {Id = 1};
            service.MarkTaskAsCompleted(completeTaskArgs);
            list.Received().MarkTaskAsCompleted(completeTaskArgs);
            }
            var args = new RenameTaskArgs {Id = 1, Name = "task name"};
        }

        public class MyModule : NinjectModule
        {
            bool result = service.TestConnection();
            {
        }

        [Fact]
        public void should_send_set_date_for_task()
        {
            var dateTime = DateTime.Now;
             var args = new SetDateArgs { Id = 1, DueDate = dateTime};
            service.SetTaskDueDate(args);
            list.Received().SetTaskDueDate(args);
        }
    }
}