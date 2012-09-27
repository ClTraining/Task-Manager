#region Using

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using EntitiesLibrary;
using FluentAssertions;
using Xunit;

#endregion


namespace TaskManagerHost.DataBaseAccessLayer
{
    public class MemoRepository : IRepository
    {

        static int newId = 0;
        private readonly List<ServiceTask> taskList;

        public MemoRepository()
        {
            taskList = new List<ServiceTask>();
        }

        public ServiceTask AddTask(ServiceTask task)
        {

            task.Id = GetNewId();

            taskList.Add(task);

            return task;
        }

        public ServiceTask GetTaskById(int id)
        {
            var task = taskList.FirstOrDefault(t => t.Id == id);

            if (task == null)
            {
                throw new Exception(String.Format("Task with id {0} was not found", id));
            }
            return task;
        }

        public List<ServiceTask> GetAllTasks()
        {
            return taskList.ToList();
        }

        public ServiceTask EditTask(ServiceTask task)
        {
            var taskToEdit = taskList.FirstOrDefault(t => t.Id == task.Id);

            if (taskToEdit == null)
            {
                throw new Exception(String.Format("Task with id {0} was not found", task.Id));
            }

            taskToEdit.Name = task.Name;

            return taskToEdit;
        }

        private int GetNewId()
        {

            if (taskList.Any())
            {
                newId = taskList.Max(x => x.Id);
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
            var task = new ServiceTask {Id = 0};
            var newtask = repository.AddTask(task);
            newtask.Id.Should().Be(1);
        }

        [Fact]
        public void should_throw_exception_when_task_was_not_found()
        {
            var repository = new MemoRepository();
            Action action = () => repository.GetTaskById(1);
            action.ShouldThrow<Exception>().WithMessage("Task with id 1 was not found");
        }

        [Fact]
        public void should_get_task_by_id()
        {
            var repository = new MemoRepository();
            repository.AddTask(new ServiceTask { Id = 10, Name = "test task" });
            repository.AddTask(new ServiceTask { Id = 0, Name = "another task" });
            repository.AddTask(new ServiceTask { Id = 0, Name = "my task" });
            var task1 = repository.GetTaskById(1);
            var task2 = repository.GetTaskById(2);
            var task3 = repository.GetTaskById(3);
            task1.Name.Should().Be("test task");
            task2.Name.Should().Be("another task");
            task3.Name.Should().Be("my task"); 
        }

        [Fact]
        public void should_throw_exception_when_task_was_not_found_for_save_task()
        {
            var repository = new MemoRepository();
            Action action = () => repository.EditTask(new ServiceTask { Id = 10, Name = "test task" });
            action.ShouldThrow<Exception>().WithMessage("Task with id 10 was not found");
        }

        [Fact]
        public void should_edit_task_by_id()
        {
            var repository = new MemoRepository();
            repository.AddTask(new ServiceTask { Id = 10, Name = "test task" });
            repository.AddTask(new ServiceTask { Id = 0, Name = "another task" });
            repository.AddTask(new ServiceTask { Id = 0, Name = "my task" });
            repository.EditTask(new ServiceTask { Id = 1, Name = "new test task" });
            repository.EditTask(new ServiceTask { Id = 2, Name = "new another task" });
            repository.EditTask(new ServiceTask { Id = 3, Name = "new my task" });
            var task1 = repository.GetTaskById(1);
            var task2 = repository.GetTaskById(2);
            var task3 = repository.GetTaskById(3);
            task1.Name.Should().Be("new test task");
            task2.Name.Should().Be("new another task");
            task3.Name.Should().Be("new my task");
        }

        [Fact]
        public void should_get_all_tasks()
        {
            var repository = new MemoRepository();
            var taskList = repository.GetAllTasks();
            taskList.Should().BeEquivalentTo(new List<ServiceTask>());
            var task1 = new ServiceTask {Id = 10, Name = "test task"};
            var task2 = new ServiceTask {Id = 10, Name = "test task1"};
            task1 = repository.AddTask(task1);
            task2= repository.AddTask(task2);
            taskList = repository.GetAllTasks();
            taskList.Should().BeEquivalentTo(new List<ServiceTask>{task1,task2});
        }
    }
}