using PMS.Demo.Utils;
using System;
using System.Configuration;

namespace PMS.Demo.Runtime
{
    public static class Global
    {
        public static string InitSql = global::PMS.Demo.Runtime.Properties.Resource.InitDatabase;
        public static string ConnectString { get{ return ConfigurationManager.ConnectionStrings["database"].ToString(); } }
        public static string CacheFilePath { get { return Environment.CurrentDirectory + @"\Photo\"; } }

        public static DBHelper dbhelper = null;
    }
}
