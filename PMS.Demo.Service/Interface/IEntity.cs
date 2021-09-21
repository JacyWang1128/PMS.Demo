using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace PMS.Demo.Service.Interface
{
    interface IEntity<T>
    {
        int Insert(T t);

        int Update(T t);

        int Delete(string id);

        T Query(string id);

        DataTable QueryList();
    }
}
