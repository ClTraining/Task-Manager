using System;
using System.Collections.Generic;
using System.Linq;
using EntitiesLibrary.CommandArguments;
using FluentAssertions;
using Specifications.ClientSpecifications;
using TaskManagerClientLibrary.ConcreteCommands.TaskFormatter;
using Xunit;

namespace TaskManagerClientLibrary
{
    class Factory : IFactory
    {
        private readonly Dictionary<Func<ListTaskArgs, bool>, Func<ListTaskArgs, IClientSpecification>> specificationDictionary;

        public Factory()
        {
            specificationDictionary = new Dictionary<Func<ListTaskArgs, bool>, Func<ListTaskArgs, IClientSpecification>>
                                          {

                                              {
                                                  x => x.DueDate != default(DateTime),
                                                  y => new ListByDateClientSpecification {Date = y.DueDate}
                                              },

                                              {
                                                  x => x.Id!= null&&x.DueDate==default(DateTime),
                                                  y => new ListSingleClientSpecification {Id = y.Id.Value}
                                              },

                                              {
                                                  x => x.DueDate == default(DateTime) && x.Id == null, 
                                                  y => new ListAllClientSpecification()
                                              }
                                          };
        }

        public IClientSpecification GetClientSpecification(ListTaskArgs listArgs)
        {
            var clientSpecification = specificationDictionary.FirstOrDefault(x => x.Key(listArgs)).Value(listArgs);
            return clientSpecification;
        }


        public ITaskFormatter GetFormatter(IClientSpecification specification)
        {
            if (specification is ListSingleClientSpecification)
                return new SingleTaskFormatter();
            return new ListTaskFormatter();
        }
    }

    public class FactoryTests
    {
        private readonly Factory factory = new Factory();
        private ListTaskArgs args;

        [Fact]
        public void when_argument_has_date_should_return_ListByDateClientSpecification()
        {
            args = new ListTaskArgs { DueDate = DateTime.Now, Id = 0 };

            var clientSpecification = factory.GetClientSpecification(args);

            clientSpecification.Should().BeOfType<ListByDateClientSpecification>();
        }

        [Fact]
        public void when_argument_has_ID_should_return_ListSingleClientSpecification()
        {
            args = new ListTaskArgs { Id = 5, DueDate = default(DateTime) };

            var clientSpecification = factory.GetClientSpecification(args);

            clientSpecification.Should().BeOfType<ListSingleClientSpecification>();
        }

        [Fact]
        public void when_argument_has_default_date_and_ID_should_return_ListAllClientSpecification()
        {
            args = new ListTaskArgs { Id = null, DueDate = default(DateTime) };
            var clientSpecification = factory.GetClientSpecification(args);

            clientSpecification.Should().BeOfType<ListAllClientSpecification>();
        }

        [Fact]
        public void should_return_list_formatter()
        {
            var result = factory.GetFormatter(new ListAllClientSpecification());
            result.Should().BeOfType<ListTaskFormatter>();
        }

        [Fact]
        public void should_return_single_formatter()
        {
            var result = factory.GetFormatter(new ListSingleClientSpecification());
            result.Should().BeOfType<SingleTaskFormatter>();
        }
    }
}