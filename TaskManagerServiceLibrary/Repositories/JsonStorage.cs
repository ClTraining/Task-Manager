using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using EntitiesLibrary;
using EntitiesLibrary.CommandArguments;
using Newtonsoft.Json;
using Specifications.ServiceSpecifications;
using TaskManagerServiceLibrary.TaskManager;

namespace TaskManagerServiceLibrary.Repositories
{
    public class JsonStorage : IRepository
    {
        private readonly ITaskMapper mapper;
        //public readonly List<ServiceTask> taskList = new List<ServiceTask>();
        private int currentId;

        public JsonStorage(ITaskMapper mapper)
        {
            this.mapper = mapper;
        }
        public int AddTask(AddTaskArgs args)
        {
            var serviceTask = new ServiceTask { Name = args.Name, DueDate = args.DueDate, Id = GetNewId() };

            AddToSaveFile(serviceTask);
            return serviceTask.Id;
        }

        private void AddToSaveFile(ServiceTask serviceTask)
        {
            var serializedTask = JsonConvert.SerializeObject(serviceTask);
            var fileStream = new FileStream("save2.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);
            var streamWriter = new StreamWriter(fileStream);
            if (fileStream.Length > 0)
            {
                fileStream.Position = fileStream.Length - 1;
                streamWriter.Write(',' + serializedTask + ']');
            }
            else
                streamWriter.Write('[' + serializedTask + ']');

            streamWriter.Close();
            fileStream.Close();
        }

        private List<ServiceTask> GetTasksFromSaveFile()
        {
            var text = File.ReadAllText("save2.txt");
            var tasks = JsonConvert.DeserializeObject<List<ServiceTask>>(text);
            return tasks;
        }

        public List<ClientPackage> GetTasks(IServiceSpecification spec)
        {
            var resList = GetTasksFromSaveFile()
                .Where(spec.IsSatisfied)
                .Select(mapper.ConvertToContract)
                .ToList();

            return resList;
        }

        public void UpdateChanges(ICommandArguments args)
        {
            var taskList = GetTasksFromSaveFile();
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

    //public class JsonStorageTests
    //{
    //    private readonly ITaskMapper mapper = Substitute.For<ITaskMapper>();
    //    private readonly IServiceSpecification spec = Substitute.For<IServiceSpecification>();
    //    readonly JsonStorage repo;
    //    readonly ServiceTask sTask = new ServiceTask { Id = 1 };

    //    public JsonStorageTests()
    //    {
    //        repo = new JsonStorage(mapper);
    //        repo.taskList.Add(sTask);
    //    }

    //    [Fact]
    //    public void should_add_task_to_repo()
    //    {
    //        var task = new AddTaskArgs { DueDate = DateTime.Today, Name = "task1" };

    //        var result = repo.AddTask(task);

    //        result.Should().Be(1);
    //    }

    //    [Fact]
    //    public void should_get_all_tasks_from_repo()
    //    {
    //        spec.IsSatisfied(sTask).Returns(true);

    //        var res = repo.GetTasks(spec);

    //        res.Count.Should().Be(1);
    //    }
    //}
}