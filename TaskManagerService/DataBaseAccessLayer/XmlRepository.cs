using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using EntitiesLibrary;

namespace TaskManagerHost.DataBaseAccessLayer
{
    class XmlRepository: IRepository
    {
        private readonly List<ServiceTask> taskList;
        private readonly XmlSerializer serializer;
        public XmlRepository()
        {
            taskList = new List<ServiceTask>();
            serializer = new XmlSerializer(typeof(List<ServiceTask>));
        }

        public ServiceTask AddTask(ServiceTask task)
        {
            throw new NotImplementedException();
        }

        public ServiceTask GetTaskById(int id)
        {
            throw new NotImplementedException();
        }

        public List<ServiceTask> GetAllTasks()
        {
            throw new NotImplementedException();
        }

        public ServiceTask EditTask(ServiceTask task)
        {
            throw new NotImplementedException();
        }
    }
}
