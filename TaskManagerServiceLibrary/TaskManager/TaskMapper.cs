using System;
using AutoMapper;
using EntitiesLibrary;
using EntitiesLibrary.CommandArguments;
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

        [Fact]
        public void test1()
        {
            var sargs = new SetDateTaskArgs{Id = 1, DueDate = DateTime.Parse("10/29/2012")};
            var rargs = new RenameTaskArgs {Id = 1, Name = "task1"};
            var task = new ServiceTask {Id = 1, DueDate = default(DateTime), Name = "123456"};
            Console.Out.WriteLine("task before = " + task.Id + task.Name + task.DueDate);
            var result1 = mapper.ConvertFromArgsToService(sargs, task);
            var result2 = mapper.ConvertFromArgsToService(rargs, task);
            Console.Out.WriteLine("result setdate = " + result1.Id + " " + result1.DueDate);
            Console.Out.WriteLine("result rename = " + result2.Id + " " + result2.Name);
            Console.Out.WriteLine("task after = " + task.Id + " " + task.Name + " " + task.DueDate);
        }
    }
}