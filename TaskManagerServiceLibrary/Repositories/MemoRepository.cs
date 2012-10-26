using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using EntitiesLibrary;
using EntitiesLibrary.CommandArguments;
using FluentAssertions;
using NSubstitute;
using Specifications.ServiceSpecifications;
using TaskManagerServiceLibrary.TaskManager;
using Xunit;

namespace TaskManagerServiceLibrary.Repositories
{
    public class MemoRepository : IRepository
    {
        private readonly ITaskMapper mapper;
        public List<ServiceTask> taskList = new List<ServiceTask>();
        private int currentId;

        public MemoRepository(ITaskMapper mapper)
        {
            this.mapper = mapper;
        }
        public int AddTask(AddTaskArgs args)
        {
            var serviceTask = new ServiceTask { Name = args.Name, DueDate = args.DueDate, Id = GetNewId() };

            taskList.Add(serviceTask);

            return serviceTask.Id;
        }

        public List<ClientPackage> GetTasks(IServiceSpecification spec)
        {
            var resList = taskList
                .Where(spec.IsSatisfied)
                .Select(mapper.ConvertToContract)
                .ToList();

            return resList;
        }

        public void UpdateChanges(ICommandArguments args)
        {
            var index = args.Id - 1;
            var taskToConvert = taskList[index];
            var task = mapper.Convert(args, taskToConvert);

            taskList.RemoveAt(index);
            taskList.Insert(index, task);
        }

        private int GetNewId()
        {
            Interlocked.Increment(ref currentId);
            return currentId;
        }
    }

    public class MemoRepositoryTests
    {
        private readonly ITaskMapper mapper = Substitute.For<ITaskMapper>();
        private readonly IServiceSpecification spec = Substitute.For<IServiceSpecification>();
        readonly MemoRepository repo;
        readonly ServiceTask sTask = new ServiceTask{Id = 1};

        public MemoRepositoryTests()
        {
            repo = new MemoRepository(mapper);
            repo.taskList.Add(sTask);
        }

        [Fact]
        public void should_add_task_to_repo()
        {
            var task = new AddTaskArgs { DueDate = DateTime.Now, Name = "task1" };
            
            var result = repo.AddTask(task);
            
            result.Should().Be(1);
        }

        [Fact]
        public void should_get_all_tasks_from_repo()
        {
            spec.IsSatisfied(sTask).Returns(true);
            
            var res = repo.GetTasks(spec);
            
            res.Count.Should().Be(1);
        }
    }
}