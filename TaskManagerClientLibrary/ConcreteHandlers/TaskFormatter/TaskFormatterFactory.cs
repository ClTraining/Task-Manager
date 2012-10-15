using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerClientLibrary.ConcreteHandlers.TaskFormatter
{
    public class TaskFormatterFactory
    {
        private SingleTaskFormatter singleTaskFormatter;
        private ListTaskFormatter listTaskFormatter;

        public TaskFormatterFactory(SingleTaskFormatter singleTaskFormatter, ListTaskFormatter listTaskFormatter)
        {
            this.listTaskFormatter = listTaskFormatter;
            this.singleTaskFormatter = singleTaskFormatter;
        }

    }
}
