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
            Mapper.CreateMap<ServiceTask, ClientPackage>();
            Mapper.CreateMap<ClientPackage, ServiceTask>();
        }

        public ClientPackage ConvertToContract(ServiceTask task)
        {
            return Mapper.Map<ServiceTask, ClientPackage>(task);
        }
    }

    public class TaskMapperTests
    {
        private readonly ClientPackage contractTask = new ClientPackage {Id = 10, Name = "service", IsCompleted = true};
        private readonly ServiceTask serviceTask = new ServiceTask {Id = 10, Name = "service", IsCompleted = true};
        private readonly TaskMapper taskMapper = new TaskMapper();

        [Fact]
        public void contract_task_should_be_equivalent_to_service_task()
        {
            var res = taskMapper.ConvertToContract(serviceTask);
            res.ShouldBeEquivalentTo(contractTask);
        }

        [Fact]
        public void should_return_null_if_null_passed_convert_from_service()
        {
            var result = taskMapper.ConvertToContract(null);
            result.Should().BeNull();
        }
    }
}