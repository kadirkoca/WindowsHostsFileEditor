using System.Security;
using static Host_Manager.ConsoleColors;

namespace Host_Manager
{
    class FileManager
    {
        private static string hostsfilepath = "C:/Windows/System32/drivers/etc/hosts";

        public static object[] Read()
        {
            string _file = "";
            IDictionary<string, string> hosts = new Dictionary<string, string>();

            Console.WriteLine("Available Definitions\r\n");

            try
            {
                string[] lines = File.ReadAllLines(hostsfilepath);
                int i = 1;

                foreach (string line in lines)
                {
                    if(line.Length > 0)
                    {
                        if (line[0] != '#')
                        {
                            string lineTxt = "line" + i.ToString();
                            Write("{=Cyan}" + lineTxt + " -> " + "{/}" + line);
                            hosts[lineTxt] = line;
                            i++;
                        }
                        _file += line + Environment.NewLine;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            object [] args = new object[2];
            args[0] = _file;
            args[1] = hosts;
            return args;
        }

        public static bool SaveFile(string newfile)
        {
            try
            {
                File.WriteAllText(hostsfilepath, newfile);
                return true;
            }
            catch (SecurityException ex)
            {
                Write("{=Red}" + ex.Message + "{/}");
            }
            catch (Exception ex)
            {
                Write("{=Red}" + ex.Message + "{/}");
            }
            return false;
        }
    }
}
