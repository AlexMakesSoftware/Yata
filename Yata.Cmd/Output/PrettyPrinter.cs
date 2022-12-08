using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YataModel;

namespace Yata.Cmd
{
    internal class PrettyPrinter
    {
        internal static void PrintTask(YTask task)
        {            
            Console.ForegroundColor = ConsoleColor.Yellow;            
            Console.WriteLine("Id: "+task.Id+" Task: " + task.Description);
            Console.ResetColor();
        }

        //print intstruction passed in
        internal static void Info(string instruction)
        {            
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(instruction);
            Console.ResetColor();
        }
        

        //print error passed in
        internal static void Error(string error)
        {            
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(error);
        }
        
        internal static void Break()
        {
            Console.WriteLine();
        }
    }
}