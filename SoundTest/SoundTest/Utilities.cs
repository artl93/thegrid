using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace SoundTest
{
    class Utilities
    {
#if DEBUG
        static StringBuilder _log = new StringBuilder(4096 * 8);
#endif

        [Conditional("DEBUG")]
        public static void FlushLog()
        {
#if DEBUG
            System.Diagnostics.Debug.WriteLine(_log.ToString());
#endif 
        }

        [Conditional("DEBUG")]
        public static void Log(string message)
        {
#if DEBUG
            _log.AppendLine(message);
#endif
        }

    }
}
