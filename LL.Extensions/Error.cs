using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LL.Extensions
{
    public class Error
    {
        public static void Export(Exception exception)
        {
            Export(exception, new Dictionary<string, object>());
        }

        public static void Export(Exception exception, Dictionary<string, object> ExceptionData)
        {
        }
    }
}
