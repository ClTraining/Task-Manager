#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using EntitiesLibrary;
using FluentAssertions;
using Xunit;

#endregion


namespace TaskManagerService.DataBaseAccessLayer
{
    public class MemoRepository : IRepository
    {
        private readonly List<ServiceTask> _taskList;

        public MemoRepository()
        {
            _taskList = new List<ServiceTask>();
        }

        public ServiceTask SaveTask(ServiceTask task)
        {

            if (task.Id == 0)
            {
                task.Id = GetNewId();
            }

            _taskList.Add(task);

            return task;
        }

        private int GetNewId()
        {
            var newId = 0;

            if (_taskList.Any())
            {
                newId = _taskList.Max(x => x.Id);
            }

            return ++newId;
        }
    }

    public class TestMemoRepository
    {
        [Fact]
        public void should_save_task_and_generate_new_id()
        {
            var repository = new MemoRepository();
            var task = new ServiceTask();
            task.Id = 0;
            var newtask = repository.SaveTask(task);
            newtask.Id.Should().Be(1);
        }
    }
}
