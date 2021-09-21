using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace PMS.Demo.Utils
{
    public static class Common
    {
        public static string GetGUID()
        {
            return Guid.NewGuid().ToString();
        }

        //public static T DataRow2POCO<T>(DataRow row,T t)
        //{
        //    var type = t.GetType();
        //    PropertyInfo[] properties = type.GetProperties();
        //    foreach (var item in properties)
        //    {
        //        type.GetProperty(item.Name).SetValue(t, row.Field<object>(item.Name),null);
        //    }
        //    return t;
        //}
    }
}
