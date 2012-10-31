using AutoMapper;
using EntitiesLibrary;
using FluentAssertions;
using Xunit;

namespace TaskManagerServiceLibrary.TaskManager
{
    public class TaskMapper : ITaskMapper
    {
        public TaskMapper()
        {
            Mapper.CreateMap<ServiceTask, ClientTask>();
        }

        public ClientTask ConvertToContract(ServiceTask task)
        {
            return Mapper.Map<ServiceTask, ClientTask>(task);
        }
    }

    public class TaskMapperTests
    {
        private readonly ClientTask contractTask = new ClientTask {Id = 10, Name = "service", IsCompleted = true};
        private readonly ServiceTask serviceTask = new ServiceTask {Id = 10, Name = "service", IsCompleted = true};
        private readonly TaskMapper mapper = new TaskMapper();

        [Fact]
        public void contract_task_should_be_equivalent_to_service_task()
        {
            var res = mapper.ConvertToContract(serviceTask);
            res.ShouldBeEquivalentTo(contractTask);
        }

        [Fact]
        public void should_return_null_if_null_passed_convert_from_service()
        {
            var result = mapper.ConvertToContract(null);
            result.Should().BeNull();
        }
    }
}