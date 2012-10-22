using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using EntitiesLibrary;
using FluentAssertions;
using Specifications.ServiceSpecifications;
using TaskManagerServiceLibrary.TaskManager;
using Xunit;

namespace TaskManagerServiceLibrary.Repositories
{
    public class MemoRepository : IRepository
    {
        private readonly ITaskMapper mapper;
        private readonly List<ServiceTask> taskList = new List<ServiceTask>();
        private int currentId;

        public MemoRepository(ITaskMapper mapper)
        {
            this.mapper = mapper;
        }

        #region IRepository Members

        public int AddTask(AddTaskArgs args)
        {
            var serviceTask = new ServiceTask {Name = args.Name, DueDate = args.DueDate, Id = GetNewId()};

            taskList.Add(serviceTask);

            return serviceTask.Id;
        }

        public List<ContractTask> GetTasks(IServiceSpecification spec)
        {
            return taskList
                .Where(spec.IsSatisfied)
                .Select(mapper.ConvertToContract)
                .ToList();
        }

        public void MarkTaskAsCompleted(CompleteTaskArgs args)
        {
        }

        public void RenameTask(RenameTaskArgs args)
        {
        }

        public void SetTaskDueDate(SetDateArgs args)
        {
        }

        #endregion

        private int GetNewId()
        {
            Interlocked.Increment(ref currentId);
            return currentId;
        }
    }

    public class MemoRepositoryTests
    {
    }
}