using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Host_Manager
{
    class ConsoleColors
    {
        public static void Write(string msg)
        {
            msg = msg + "\n";
            string[] ss = msg.Split('{', '}');
            ConsoleColor c;
            foreach (var s in ss)
                if (s.StartsWith("/"))
                    Console.ResetColor();
                else if (s.StartsWith("=") && Enum.TryParse(s.Substring(1), out c))
                    Console.ForegroundColor = c;
                else
                    Console.Write(s);
        }
    }
}
