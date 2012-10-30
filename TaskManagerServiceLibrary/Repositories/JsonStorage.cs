using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using EntitiesLibrary;
using EntitiesLibrary.CommandArguments;
using FluentAssertions;
using Newtonsoft.Json;
using Specifications.ServiceSpecifications;
using TaskManagerServiceLibrary.TaskManager;
using Xunit;

namespace TaskManagerServiceLibrary.Repositories
{
    public class JsonStorage : IRepository
    {
        private readonly ITaskMapper mapper;
        private readonly List<ServiceTask> cacheStorage;
        private int currentId;

        public JsonStorage(ITaskMapper mapper)
        {
            this.mapper = mapper;
            var jsonString = GetJsonString();
            cacheStorage = DeserializeToListOfTasks(jsonString);
        }
        public int AddTask(AddTaskArgs args)
        {
            var serviceTask = new ServiceTask { Name = args.Name, DueDate = args.DueDate, Id = GetNewId() };
            AddToCache(serviceTask);
            SynchronizeCacheAndLocal();
            return serviceTask.Id;
        }

        public List<ClientPackage> GetTasks(IServiceSpecification spec)
        {
            var resList = cacheStorage
                .Where(spec.IsSatisfied)
                .Select(mapper.ConvertToContract)
                .ToList();

            return resList;
        }

        public void UpdateChanges(ICommandArguments args)
        {
            var index = args.Id - 1;
            var taskToConvertCache = cacheStorage[index];
            var task = mapper.Convert(args, taskToConvertCache);
            UpdateInCache(index, task);
            SynchronizeCacheAndLocal();
        }

        private void AddToCache(ServiceTask serviceTask)
        {
            cacheStorage.Add(serviceTask);
        }

        private string GetJsonString()
        {
            string text;
            try
            {
                text = File.ReadAllText("save.txt");
            }
            catch (FileNotFoundException)
            {
                text = "[]";
            }
            DeserializeToListOfTasks(text);
            return text;
        }

        private List<ServiceTask> DeserializeToListOfTasks(string text)
        {
            return JsonConvert.DeserializeObject<List<ServiceTask>>(text);
        }

        private void SynchronizeCacheAndLocal()
        {
            var stream = new FileStream("save.txt", FileMode.Create);
            var writer = new StreamWriter(stream);
            var serializeObject = JsonConvert.SerializeObject(cacheStorage);
            writer.Write(serializeObject);
            writer.Close();
            stream.Close();
        }

        private void UpdateInCache(int index, ServiceTask task)
        {
            cacheStorage.RemoveAt(index);
            cacheStorage.Insert(index, task);
        }

        private int GetNewId()
        {
            Interlocked.Increment(ref currentId);
            return currentId;
        }
    }

    public class JsonStorageTests
    {
        private readonly ITaskMapper mapper = NSubstitute.Substitute.For<ITaskMapper>();
        private JsonStorage storage;

        public JsonStorageTests()
        {
            storage = new JsonStorage(mapper);
        }

        [Fact]
        public void should_write_task_to_save_file()
        {
            var args = new AddTaskArgs { DueDate = DateTime.Today, Name = "sasha" };
            storage.AddTask(args);
            var text = File.ReadAllText("save.txt");
            text.Should().NotBeEmpty();
        }

        [Fact]
        public void should_get_tasks_by_specification()
        {
            IServiceSpecification specification = new ListAllServiceSpecification();
            var tasks = storage.GetTasks(specification);

            tasks.Count.Should().BeGreaterOrEqualTo(1);
        }
    }
}