using System.Text;
using System.Threading.Tasks;
using EntitiesLibrary.CommandArguments;
using Specifications.ClientSpecifications;
using TaskManagerClientLibrary.ConcreteCommands.TaskFormatter;

namespace TaskManagerClientLibrary
{
    public interface IFactory
    {
        IClientSpecification GetClientSpecification(ListTaskArgs listArgs);
        ITaskFormatter GetFormatter(IClientSpecification specification);
    }
}
