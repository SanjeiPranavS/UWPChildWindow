using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTeachingTip
{
    internal static class Extension
    {
        public static void Print(this string message)
        {
            Debug.WriteLine(message);
        }

    }
}
