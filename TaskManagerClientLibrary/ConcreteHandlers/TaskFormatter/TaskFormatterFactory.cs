using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerClientLibrary.ConcreteHandlers.TaskFormatter
{
    public class TaskFormatterFactory
    {
        private readonly SingleTaskFormatter singleTaskFormatter;
        private readonly ListTaskFormatter listTaskFormatter;

        public TaskFormatterFactory(SingleTaskFormatter singleTaskFormatter, ListTaskFormatter listTaskFormatter)
        {
            this.listTaskFormatter = listTaskFormatter;
            this.singleTaskFormatter = singleTaskFormatter;
        }

        public virtual ITaskFormatter GetListFormatter()
        {
            return listTaskFormatter;
        }

        public virtual ITaskFormatter GetSingleFormatter()
        {
            return singleTaskFormatter;
        }
    }

    public class TaskFormatterFactoryTests
    {

    }
}
