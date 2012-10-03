using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using EntitiesLibrary;
using FluentAssertions;
using TaskManagerHost.WCFServer;
using Xunit;

namespace TaskManagerHost.DataBaseAccessLayer
{
    public class MemoRepository : IRepository
    {
        readonly List<ServiceTask> taskList = new List<ServiceTask>();
        private int currentId;

        public int AddTask(string name)
        {
            var serviceTask = new ServiceTask { Name = name, Id = GetNewId() };

            taskList.Add(serviceTask);

            return serviceTask.Id;
        }

        public ServiceTask GetTaskById(int id)
        {
            ServiceTask task=null;
            try
            {
                task = taskList.FirstOrDefault(t => t.Id == id);
                if (task == null)
                    throw new NullReferenceException();
            }
            catch (NullReferenceException)
            {
                Console.WriteLine("Sorry, but Task with this ID was not found");
            }
            return task;
        }

        public List<ServiceTask> GetAllTasks()
        {
            return taskList.ToList();
        }

        public void MarkCompleted(int id)
        {
            GetTaskById(id).IsCompleted = true;
        }

        private int GetNewId()
        {
            Interlocked.Increment(ref currentId);
            return currentId;
        }
    }

    public class MemoRepositoryTests
    {
        private readonly List<string> taskNames = new List<string> { "test task", "another task", "my task" };

        [Fact]
        public void should_save_task_and_generate_new_id()
        {
            var repository = new MemoRepository();
            var tTask = repository.AddTask(taskNames[0]);
            tTask.Should().Be(1);
        }

        [Fact]
        public void should_get_task_by_id()
        {
            var repository = new MemoRepository();
            var addedTasks = taskNames.Select(repository.AddTask);
            var receivedTasks = addedTasks.Select(repository.GetTaskById).ToList();
            receivedTasks.Select(x => x.Name.Should().Be(taskNames.ToArray()[receivedTasks.ToList().IndexOf(x)]));
        }

        [Fact]
        public void should_edit_task_by_id()
        {
            var repository = new MemoRepository();
            var addedTaskIds = taskNames.Select(repository.AddTask).ToList();
            foreach (var addedTaskId in addedTaskIds)
            {
                repository.MarkCompleted(addedTaskId);
            }
            var receivedTasks = addedTaskIds.Select(repository.GetTaskById).ToList();
            foreach (var task in receivedTasks)
            {
                task.IsCompleted.Should().Be(true);
            }
        }

        [Fact]
        public void should_return_empty_list()
        {
            var repository = new MemoRepository();
            var taskList = repository.GetAllTasks();
            taskList.Should().BeEquivalentTo(new List<ServiceTask>());
        }

        [Fact]
        public void should_get_all_tasks()
        {
            var repository = new MemoRepository();
            taskNames.Select(repository.AddTask).ToList();
            var taskList = repository.GetAllTasks().ToList();
            foreach (var task in taskList)
            {
                task.Name.Should().Be(taskNames.ToArray()[taskList.IndexOf(task)]);
            }
        }

    }
}