using System.Diagnostics;
using static Host_Manager.ConsoleColors;

namespace Host_Manager
{
    class Program
    {
        private static void Main(string[] args)
        {

            Console.WriteLine("Welcome to host manager\r");
            Console.WriteLine("------------------------\n");

            object[] hostsfile = FileManager.Read();
            if (hostsfile == null) return;

            if(hostsfile != null && hostsfile.Length > 0)
            {
                Console.BackgroundColor = ConsoleColor.Green;
                Write("\n" + "{=Black}Use new, edit, delete, cancel functions to handle hosts file{/}");
                Write("\n Example usages");
                Write("----------------------");
                Write("{=Green}new newvalue{/}");
                Write("{=Green}edit line1 newvalue{/}");
                Write("{=Green}save{/}");
                Write("{=Red}delete line1{/}");
                Write("{=Yellow}cancel{/}");
                Write("----------------------"+"\n");
            }
            string input = Console.ReadLine();
            input = input == null ? "" : input.Trim();
            if (input == "") return;
            string[] result = getTheRest(input);
            validateCommand(result, hostsfile);
        }

        private static string[] getTheRest(string incoming)
        {
            int firstCommaIndex = incoming.IndexOf(' ');
            firstCommaIndex = firstCommaIndex == -1 ? 0 : firstCommaIndex;

            string firstPart = incoming.Substring(0, firstCommaIndex);
            string secondPart = incoming.Substring(firstCommaIndex + 1);
            if (firstPart == "") firstPart = incoming;

            return new string[] { firstPart, secondPart };
        }

        private static void validateCommand(string[] commandline, object[] hostsfile)
        {
            string originalHostsFile = (string)hostsfile[0];
            IDictionary<string, string> hosts = (IDictionary<string, string>)hostsfile[1];

            string command = commandline[0];
            string rest = commandline[1].Trim();

            if (command == null) return;
            if(command == "new")
            {
                if(rest == "")
                {
                    Write("{=Yellow} Warning! value is no valid!{/}");
                }
                else
                {
                    originalHostsFile += rest;
                    string lineNumberText = "line" + hosts.Count().ToString();
                    Write("{=Yellow} A new value has defined into temporary file! To keep it permanent, please run 'save' command{/}");
                    Write("{=Cyan}" + lineNumberText + "{/} " + rest);
                    hosts[lineNumberText] = rest;
                }
            }
            else if (command == "edit")
            {
                string[] result = getTheRest(rest);
                string lineNumber = result[0];
                string newvalue = result[1].Trim();
                bool keyExists = hosts.ContainsKey(lineNumber);

                if (newvalue != "" && keyExists && hosts[lineNumber] != "")
                {
                    string oldvalue = hosts[lineNumber].ToString();

                    originalHostsFile = originalHostsFile.Replace(oldvalue, newvalue);
                    Write("{=Yellow} The value has been updated on temporary file! To keep it permanent, please run 'save' command{/}");
                    Write("{=Cyan}" + lineNumber + "{/} " + newvalue);
                    hosts[lineNumber] = newvalue;
                }
                else
                {
                    Write("{=Yellow} Warning! line number parameter is not valid!{/} Given : "+ lineNumber);
                }
            }
            else if (command == "delete")
            {
                string[] result = getTheRest(rest);
                string lineNumber = result[0];
                bool keyExists = hosts.ContainsKey(lineNumber);

                if (keyExists && hosts[lineNumber] != "")
                {
                    string oldvalue = hosts[lineNumber].ToString();

                    originalHostsFile = originalHostsFile.Replace(oldvalue, "");
                    Write("{=Yellow} The value has been deleted from temporary file! To keep it permanent, please run 'save' command{/}");
                    hosts.Remove(lineNumber);
                }
                else
                {
                    Write("{=Yellow} Warning! Command is no valid!{/}");
                }
            }
            else if (command == "save")
            {
                bool isSaved = FileManager.SaveFile(originalHostsFile);
                if (isSaved)
                {
                    Write("{=Green} Succeed! Hosts file has been saved successfully!{/}");
                }
            }
            
            if (command == "cancel")
            {
                Environment.Exit(0);
            }

            string input = Console.ReadLine();
            input = input == null ? "" : input.Trim();
            if (input == "") return;
            string[] resultParams = getTheRest(input);

            object[] args = new object[2];
            args[0] = originalHostsFile;
            args[1] = hosts;
            validateCommand(resultParams, args);
        }
    }
}