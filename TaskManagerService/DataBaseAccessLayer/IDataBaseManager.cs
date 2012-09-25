#region 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerServise.Entities;

#endregion


namespace TaskManagerServise.DataBaseAccessLayer
{
    public interface IDataBaseManager
    {
        CTask SaveTask(CTask task);
    }
}
