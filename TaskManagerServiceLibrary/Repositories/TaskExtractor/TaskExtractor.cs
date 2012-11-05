using System.Collections.Generic;
using System.Linq;
using CommandQueryLibrary.ServiceSpecifications;
using EntitiesLibrary;
using NSubstitute;
using FluentAssertions;
using Xunit;

namespace TaskManagerServiceLibrary.Repositories.TaskExtractor
{
    public class TaskExtractor : ITaskExtractor
    {
        private readonly IRepository repo;

        public TaskExtractor(IRepository repository)
        {
            repo = repository;
        }

        public ServiceTask SelectTaskById(int id)
        {
            var task = repo.GetTasks(new ListSingleServiceSpecification { Id = id }).FirstOrDefault();
            if (task == null) throw new TaskNotFoundException(id);
            return task;
        }
    }

    public class TaskExtractorTester
    {
        private readonly IRepository repo = Substitute.For<IRepository>();
        private readonly TaskExtractor extractor;

        public TaskExtractorTester()
        {
            extractor = new TaskExtractor(repo);
        }

        [Fact]
        public void should_return_task_with_given_id()
        {
            const int id = 5;
            var serviceTask = new ServiceTask { Id = id };
            repo.GetTasks(Arg.Any<ListSingleServiceSpecification>()).Returns(new List<ServiceTask> { serviceTask });

            var selectTaskById = extractor.SelectTaskById(id);

            selectTaskById.Id.Should().Be(id);
        }
    }
}
