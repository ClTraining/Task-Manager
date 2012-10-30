using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntitiesLibrary.CommandArguments
{
    public interface IEditCommandArguments: ICommandArguments
    {
        int Id { get; set; }
    }
}
