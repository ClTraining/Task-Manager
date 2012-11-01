using System;
using AutoMapper;
using EntitiesLibrary.CommandArguments;
using FluentAssertions;
using NSubstitute;
using Ninject;
using TaskManagerServiceLibrary.Commands;
using TaskManagerServiceLibrary.Repositories;
using TaskManagerServiceLibrary.TaskManager;
using Xunit;

namespace TaskManagerServiceLibrary
{
    public interface IArgToCommandConverter
    {
        IServiceCommand GetServiceCommand(IEditCommandArguments args);
    }

    public class ArgToCommandConverter : IArgToCommandConverter
    {
        public ArgToCommandConverter(IKernel kernel)
        {

            Mapper.Initialize(map => map.ConstructServicesUsing(t => kernel.Get(t)));

            Mapper.CreateMap<CompleteTaskArgs, CompleteServiceCommand>()
                .ConstructUsing((ResolutionContext a) => kernel.Get<CompleteServiceCommand>());

            Mapper.CreateMap<IEditCommandArguments, IServiceCommand>().ConvertUsing<MapConverter<IEditCommandArguments, IServiceCommand>>();
        }

        public IServiceCommand GetServiceCommand(IEditCommandArguments args)
        {
            return Mapper.Map<IServiceCommand>(args);
        }
    }

    public class ArgToCommandConverterTests
    {
        [Fact]
        public void test1()
        {
            var kernel = new StandardKernel();
            kernel.Bind<ITodoList>().ToConstant(Substitute.For<ITodoList>());
            var args = new CompleteTaskArgs {Id = 1};
            var converter = new ArgToCommandConverter(kernel);
            var result = (CompleteServiceCommand)converter.GetServiceCommand(args);

            result.Id.Should().Be(1);
        }
    }
}
