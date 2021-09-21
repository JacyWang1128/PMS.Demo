using PMS.Demo.Client;
using PMS.Demo.Runtime;
using PMS.Demo.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace PMS.Demo
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            #region 初始化数据库

            if (Runtime.Global.dbhelper == null)
            {

                Runtime.Global.dbhelper = new SQLiteHelper(Runtime.Global.ConnectString);

                //若数据库不存在（第一次使用/数据文件被删，则自动创建数据库）
                if (!Runtime.Global.dbhelper.TestConnection())
                {
                    var array = Runtime.Global.ConnectString.Split(';');
                    foreach (var item in array)
                    {
                        string database = Regex.Replace(item, @"\s", "");
                        if (database.StartsWith("DataSource="))
                        {
                            string databasepath = database.Remove(0, "DataSource=".Count());
                            Runtime.Global.dbhelper.CreateDatabase(databasepath);
                        }

                    }
                }
                if (!Runtime.Global.dbhelper.TableExist("t_person"))
                {
                    Runtime.Global.dbhelper.ExecuteNonQuery(Runtime.Global.InitSql);
                }
            }

            #endregion

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
