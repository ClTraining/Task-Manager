using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using EntitiesLibrary;
using FluentAssertions;
using Xunit;

namespace TaskManagerHost.DataBaseAccessLayer
{
    class XmlRepository: IRepository
    {
        private readonly string fileName;
        private List<ServiceTask> TaskList {
            get
            {
                var fileStream = new FileStream(fileName, FileMode.Open);
                var retu = (List<ServiceTask>) serializer.Deserialize(fileStream);
                fileStream.Close();
                return retu;
            }
            set
            {
                var myWriter = new StreamWriter(fileName);
                serializer.Serialize(myWriter, value);
                myWriter.Close();
            }
        }
        private readonly List<ServiceTask> taskList;
        private readonly XmlSerializer serializer;
        public XmlRepository(string fileName)
        {
            this.fileName = fileName;

            serializer = new XmlSerializer(typeof(List<ServiceTask>));

            if (!File.Exists(fileName))
            {
                TaskList = new List<ServiceTask>();
            }
            taskList = TaskList;
        }

        public ServiceTask AddTask(ServiceTask task)
        {

            task.Id = GetNewId();

            taskList.Add(task);
            TaskList = taskList;

            return task;
        }

        public int AddTask(string name)
        {
            throw new NotImplementedException();
        }

        public ServiceTask GetTaskById(int id)
        {
            var task = TaskList.FirstOrDefault(t => t.Id == id);

            if (task == null)
            {
                throw new Exception(String.Format("Task with id {0} was not found", id));
            }
            return task;
        }

        public List<ServiceTask> GetAllTasks()
        {
            return TaskList.ToList();
        }

        public bool MarkCompleted(int id)
        {
            throw new NotImplementedException();
        }

        public bool DeleteAllTasks()
        {
            throw new NotImplementedException();
        }

        public ServiceTask EditTask(ServiceTask task)
        {
            var taskToEdit = taskList.FirstOrDefault(t => t.Id == task.Id);

            if (taskToEdit == null)
            {
                throw new Exception(String.Format("Task with id {0} was not found", task.Id));
            }

            taskToEdit.Name = task.Name;
            TaskList = taskList;

            return taskToEdit;
        }

        public int GetNewId()
        {
            var newId = 0;

            if (TaskList.Any())
            {
                newId = TaskList.Max(x => x.Id);
            }

            return ++newId;
        }
    }
    public class XmlRepositoryTests
    {
        [Fact]
        public void should_save_task_and_generate_new_id()
        {
            var repository = new XmlRepository("test1");
            var task = new ServiceTask { Id = 0 };
            var newtask = repository.AddTask(task);
            newtask.Id.Should().Be(1);
            File.Delete("test1");
        }

        [Fact]
        public void should_throw_exception_when_task_was_not_found()
        {
            var repository = new XmlRepository("test2");
            Action action = () => repository.GetTaskById(1);
            action.ShouldThrow<Exception>().WithMessage("Task with id 1 was not found");
            File.Delete("test2");
        }

        [Fact]
        public void should_get_task_by_id()
        {
            var repository = new XmlRepository("test3");
            repository.AddTask(new ServiceTask { Id = 10, Name = "test task" });
            repository.AddTask(new ServiceTask { Id = 0, Name = "another task" });
            repository.AddTask(new ServiceTask { Id = 0, Name = "my task" });
            var task1 = repository.GetTaskById(1);
            var task2 = repository.GetTaskById(2);
            var task3 = repository.GetTaskById(3);
            task1.Name.Should().Be("test task");
            task2.Name.Should().Be("another task");
            task3.Name.Should().Be("my task");
            File.Delete("test3");
        }

        [Fact]
        public void should_throw_exception_when_task_was_not_found_for_save_task()
        {
            var repository = new XmlRepository("test4");
            Action action = () => repository.EditTask(new ServiceTask { Id = 10, Name = "test task" });
            action.ShouldThrow<Exception>().WithMessage("Task with id 10 was not found");
            File.Delete("test4");
        }

        [Fact]
        public void should_edit_task_by_id()
        {
            var repository = new XmlRepository("test5");
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
            File.Delete("test5");
        }

        [Fact]
        public void should_get_all_tasks()
        {
            var repository = new XmlRepository("test6");
            var taskList = repository.GetAllTasks();
            taskList.Should().BeEquivalentTo(new List<ServiceTask>());
            var task1 = new ServiceTask { Id = 10, Name = "test task" };
            var task2 = new ServiceTask { Id = 10, Name = "test task1" };
            task1 = repository.AddTask(task1);
            task2 = repository.AddTask(task2);
            taskList = repository.GetAllTasks();
            taskList.Count.Should().Be(2);
            taskList.First().Name.Should().Be(task1.Name);
            taskList.Last().Name.Should().Be(task2.Name);
            File.Delete("test6");
        }
    }
}
