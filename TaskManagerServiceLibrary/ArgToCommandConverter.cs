using AutoMapper;
using EntitiesLibrary.CommandArguments;
using FluentAssertions;
using NSubstitute;
using Ninject;
using TaskManagerServiceLibrary.Commands;
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

            //            Mapper.Initialize(map => map.ConstructServicesUsing(t => kernel.Get(t)));

            Mapper.CreateMap<ClearDateTaskArgs, ClearDateServiceCommand>()
                .ConstructUsing((ClearDateTaskArgs a) => kernel.Get<ClearDateServiceCommand>());

            Mapper.CreateMap<CompleteTaskArgs, CompleteServiceCommand>()
                .ConstructUsing((CompleteTaskArgs a) => kernel.Get<CompleteServiceCommand>());

            Mapper.CreateMap<RenameTaskArgs, RenameServiceCommand>()
                .ConstructUsing((RenameTaskArgs a) => kernel.Get<RenameServiceCommand>());

            Mapper.CreateMap<SetDateTaskArgs, SetDateServiceCommand>()
                .ConstructUsing((SetDateTaskArgs a) => kernel.Get<SetDateServiceCommand>());

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
            var args = new CompleteTaskArgs { Id = 1 };
            var converter = new ArgToCommandConverter(kernel);
            var result = (CompleteServiceCommand)converter.GetServiceCommand(args);

            result.Id.Should().Be(1);
        }
    }
}
