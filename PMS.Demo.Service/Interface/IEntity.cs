using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMS.Demo.Service.Interface
{
    interface IEntity<T>
    {
        int Insert(T t);

        int Update(T t);

        int Delete(T t);

        T Query(string id);

        List<T> QueryList();
    }
}
