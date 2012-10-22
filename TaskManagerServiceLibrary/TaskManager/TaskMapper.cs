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
            Mapper.CreateMap<ServiceTask, ContractTask>();
            Mapper.CreateMap<ContractTask, ServiceTask>();
        }

        #region ITaskMapper Members

        public ContractTask ConvertToContract(ServiceTask task)
        {
            return Mapper.Map<ServiceTask, ContractTask>(task);
        }

        #endregion
    }

    public class TaskMapperTests
    {
        private readonly ContractTask contractTask = new ContractTask {Id = 10, Name = "service", IsCompleted = true};
        private readonly ServiceTask serviceTask = new ServiceTask {Id = 10, Name = "service", IsCompleted = true};
        private readonly TaskMapper taskMapper = new TaskMapper();

        [Fact]
        public void contract_task_should_be_equivalent_to_service_task()
        {
            ContractTask res = taskMapper.ConvertToContract(serviceTask);
            res.ShouldBeEquivalentTo(contractTask);
        }

        [Fact]
        public void should_return_null_if_null_passed_convert_from_service()
        {
            ContractTask result = taskMapper.ConvertToContract(null);
            result.Should().BeNull();
        }
    }
}