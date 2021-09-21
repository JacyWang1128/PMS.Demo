using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMS.Demo.Utils
{
    public static class Common
    {
        public static string GetGUID()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
