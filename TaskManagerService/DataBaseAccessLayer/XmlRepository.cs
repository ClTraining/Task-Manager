using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using EntitiesLibrary;
using FluentAssertions;
using Xunit;

namespace TaskManagerHost.DataBaseAccessLayer
{
    class XmlRepository: IRepository
    {
        private const string FileName = "e:\\test.xml";
        private List<ServiceTask> TaskList { get {var fileStream= new FileStream(FileName, FileMode.Open);
            var retu = (List<ServiceTask>)serializer.Deserialize(fileStream);
            fileStream.Close();
            return retu;
        }
            set
            {
                var myWriter = new StreamWriter(FileName);
                serializer.Serialize(myWriter, value);
                myWriter.Close();
            }
        }
        private List<ServiceTask> taskList;
        private readonly XmlSerializer serializer;
        public XmlRepository()
        {
            serializer = new XmlSerializer(typeof(List<ServiceTask>));
            TaskList = new List<ServiceTask>();
            taskList = new List<ServiceTask>();
        }

        public ServiceTask AddTask(ServiceTask task)
        {

            task.Id = GetNewId();

            taskList.Add(task);
            TaskList = taskList;

            return task;
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

        private int GetNewId()
        {
            var newId = 0;

            if (TaskList.Any())
            {
                newId = TaskList.Max(x => x.Id);
            }

            return ++newId;
        }
    }
    public class TestXmlRepository
    {
        [Fact]
        public void should_save_task_and_generate_new_id()
        {
            var repository = new XmlRepository();
            var task = new ServiceTask { Id = 0 };
            var newtask = repository.AddTask(task);
            newtask.Id.Should().Be(1);
        }

        [Fact]
        public void should_throw_exception_when_task_was_not_found()
        {
            var repository = new XmlRepository();
            Action action = () => repository.GetTaskById(1);
            action.ShouldThrow<Exception>().WithMessage("Task with id 1 was not found");
        }

        [Fact]
        public void should_get_task_by_id()
        {
            var repository = new XmlRepository();
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
            var repository = new XmlRepository();
            Action action = () => repository.EditTask(new ServiceTask { Id = 10, Name = "test task" });
            action.ShouldThrow<Exception>().WithMessage("Task with id 10 was not found");
        }

        [Fact]
        public void should_edit_task_by_id()
        {
            var repository = new XmlRepository();
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
            var repository = new XmlRepository();
            var taskList = repository.GetAllTasks();
            taskList.Should().BeEquivalentTo(new List<ServiceTask>());
            var task1 = new ServiceTask { Id = 10, Name = "test task" };
            var task2 = new ServiceTask { Id = 10, Name = "test task1" };
            task1 = repository.AddTask(task1);
            task2 = repository.AddTask(task2);
            taskList = repository.GetAllTasks();
            taskList.Should().BeEquivalentTo(new List<ServiceTask> { task1, task2 });
        }
    }
}
