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

        public ServiceTask ConvertToService(ContractTask task)
        {
            return Mapper.Map<ContractTask, ServiceTask>(task);
        }

        public ContractTask ConvertToContract(ServiceTask task)
        {
            return Mapper.Map<ServiceTask, ContractTask>(task);
        }
    }

    public class TaskMapperTests
    {
        private readonly ServiceTask serviceTask = new ServiceTask { Id = 10, Name = "service", IsCompleted = true };
        private readonly ContractTask contractTask = new ContractTask { Id = 11, Name = "contract", IsCompleted = false };
        private readonly TaskMapper taskMapper = new TaskMapper();

        [Fact]
        public void should_convert_service_to_contract()
        {
            var result = taskMapper.ConvertToContract(serviceTask);
            result.Should().BeOfType<ContractTask>();
        }

        [Fact]
        public void should_have_same_name_convert_service_to_contract()
        {
            var result = taskMapper.ConvertToContract(serviceTask);
            result.Name.Should().Be(serviceTask.Name);
        }

        [Fact]
        public void should_have_same_id_convert_service_to_contract()
        {
            var result = taskMapper.ConvertToContract(serviceTask);
            result.Id.Should().Be(serviceTask.Id);
        }

        [Fact]
        public void should_have_same_isCompleted_convert_service_to_contract()
        {
            var result = taskMapper.ConvertToContract(serviceTask);
            result.IsCompleted.Should().Be(serviceTask.IsCompleted);
        }

        [Fact]
        public void should_convert_contract_to_service()
        {
            var result = taskMapper.ConvertToService(contractTask);
            result.Should().BeOfType<ServiceTask>();
        }

        [Fact]
        public void should_have_same_name_convert_contract_to_service()
        {
            var result = taskMapper.ConvertToService(contractTask);
            result.Name.Should().Be(contractTask.Name);
        }

        [Fact]
        public void should_have_same_id_convert_contract_to_service()
        {
            var result = taskMapper.ConvertToService(contractTask);
            result.Id.Should().Be(contractTask.Id);
        }

        [Fact]
        public void should_have_same_isCompleted_convert_contract_to_service()
        {
            var result = taskMapper.ConvertToService(contractTask);
            result.IsCompleted.Should().Be(contractTask.IsCompleted);
        }

        [Fact]
        public void should_return_null_if_null_passed_convert_from_contract()
        {
            var result = taskMapper.ConvertToService(null);
            result.Should().BeNull();
        }

        [Fact]
        public void should_return_null_if_null_passed_convert_from_service()
        {
            var result = taskMapper.ConvertToContract(null);
            result.Should().BeNull();
        }

    }
}
