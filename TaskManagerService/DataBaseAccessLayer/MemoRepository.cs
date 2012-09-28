using System;
using System.Collections.Generic;
using System.Linq;
using EntitiesLibrary;
using FluentAssertions;
using Xunit;


namespace TaskManagerHost.DataBaseAccessLayer
{
    public class MemoRepository : IRepository
    {

        static List<ServiceTask> taskList = new List<ServiceTask>();

        public ServiceTask AddTask(ServiceTask task)
        {

            task.Id = GetNewId();

            taskList.Add(task);

            return task;
        }

        public ServiceTask GetTaskById(int id)
        {
            var task = taskList.FirstOrDefault(t => t.Id == id);

            //try
            //{
            //    if (task == null)
            //        throw new Exception(String.Format("Task with id {0} was not found", id));
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e.Message);
            //}

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

        public int GetNewId()
        {
           var newId = 0;

            if (taskList.Any())
            {
                newId = taskList.Max(x => x.Id);
            }

            return ++newId;
        }
    }

    public class TestMemoRepository
    {
        public void should_save_task_and_generate_new_id()
        {
            var repository = new MemoRepository();
            var task = new ServiceTask { Id = 0 };
            var tTask = repository.AddTask(task);
            Console.Out.WriteLine(tTask.Id);
            tTask.Id.Should().Be(1);
        }

        public void should_throw_exception_when_task_was_not_found()
        {
            var repository = new MemoRepository();
            Action action = () => repository.GetTaskById(1);
            action.ShouldThrow<Exception>().WithMessage("Task with id 1 was not found");
        }

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

        public void should_throw_exception_when_task_was_not_found_for_save_task()
        {
            var repository = new MemoRepository();
            Action action = () => repository.EditTask(new ServiceTask { Id = 10, Name = "test task" });
            action.ShouldThrow<Exception>().WithMessage("Task with id 10 was not found");
        }

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

        private List<ServiceTask> list = new List<ServiceTask>
                                             {
                                                 new ServiceTask{Id = 1, Name = "first test"},
                                                 new ServiceTask{Id = 2, Name = "second test"},
                                                 new ServiceTask{Id= 3, Name = "third test"}
                                             };

        MemoRepository memo = new MemoRepository();

        [Fact]
        public void FactMethodName()
        {
            list.ForEach(x => memo.AddTask(x));

            var res = memo.GetAllTasks();
            res.Count.Should().Be(3);
        }

        [Fact]
        public void FactMethodName1()
        {
            
        }
    }
}