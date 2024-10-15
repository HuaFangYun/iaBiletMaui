using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iaBilet.Core.Lib
{
    public class Log
    {
        public static void WriteLine(string message)
        {
            if (Debugger.IsAttached)
            {
#if NETCORE
            Debug.WriteLine(message);
#else
                Trace.WriteLine(message);
#endif
            }
        }

        public static void WriteException(Exception ex)
        {
            if (Debugger.IsAttached)
            {
#if NETCORE
            Debug.WriteLine(ex.Message);
#else
                Trace.WriteLine("Exception" + ex.Message);
#endif
            }
        }
    }

}
