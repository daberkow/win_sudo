/*
 * Dan Berkowitz, Jan 2013
 * 
 * Simple app to drop in system32 to be able to elevate commands easily
 * 
 * Thanks to http://stackoverflow.com/questions/7351429/how-to-elevate-net-application-permissions
 * 
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace sudo
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Please enter a command to run");
                return;
            }
            else
            {
                try
                {
                    ProcessStartInfo startInfo = new ProcessStartInfo();
                    startInfo.UseShellExecute = true;
                    startInfo.WorkingDirectory = Environment.CurrentDirectory;
                    startInfo.FileName = args[0];
                    string temp_string = "";
                    for (int i = 1; i < args.Length; i++)
                    {
                        temp_string += args[i] + " ";
                    }
                    startInfo.Arguments = temp_string;
                    startInfo.Verb = "runas";
                    Process p = Process.Start(startInfo);
                    return;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}
