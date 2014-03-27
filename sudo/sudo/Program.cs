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
using System.Security.Principal;

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
                if (args[0] == "/write")
                {
                    var identity = WindowsIdentity.GetCurrent();
                    var principal = new WindowsPrincipal(identity);

                    if (principal.IsInRole(WindowsBuiltInRole.Administrator))
                    {
                        bool ErrorHasOccured = false;
                        ErrorHasOccured = writeLsCommand();

                        if (ErrorHasOccured)
                        {
                            ErrorHasOccured = writeIfconfig();
                        }

                        if (ErrorHasOccured)
                        {
                            ErrorHasOccured = writesuperc();
                        }
                    }
                    else
                    {
                        Console.WriteLine("This command needs to be run from a administrative mode");
                    }
                }else{
                    if(args[0] == "/?")
                    {
                        System.Console.WriteLine("Win Sudo - Dan Berkowitz, v2, https://github.com/daberkow/win_sudo");
                        System.Console.WriteLine("Run applications in Windows command prompt as administrator");
                        System.Console.WriteLine("");
                        System.Console.WriteLine("Usage: sudo [program] [/write]");
                        System.Console.WriteLine("");
                        System.Console.WriteLine("/write option (requires admin) writes ls, ifconfig, and superc to cmd");
                    }else{
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

        private static bool writeLsCommand()
        {
            //ls
            string cmd = "dir %1";
            string windir = System.Environment.GetEnvironmentVariable("windir");
            System.IO.FileInfo fileWrite = new System.IO.FileInfo(windir + "\\system32\\ls.cmd");
            if (fileWrite.Exists)
            {
                Console.WriteLine("File ls.cmd exists, skipping");
                return true;
            }
            else
            {
                try
                {
                    System.IO.StreamWriter writer = fileWrite.CreateText();
                    writer.WriteLine(cmd);
                    writer.Flush();
                    writer.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error writing ls");
                    return false;
                }
                Console.WriteLine("ls written");
                return true;
            }
        }

        private static bool writeIfconfig()
        {
            //ifconfig
            string cmd = "ipconfig %1";
            string windir = System.Environment.GetEnvironmentVariable("windir");
            System.IO.FileInfo fileWrite = new System.IO.FileInfo(windir + "\\system32\\ifconfig.cmd");
            if (fileWrite.Exists)
            {
                Console.WriteLine("File ifconfig.cmd exists, skipping");
                return true;
            }
            else
            {
                try
                {
                    System.IO.StreamWriter writer = fileWrite.CreateText();
                    writer.WriteLine(cmd);
                    writer.Flush();
                    writer.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error writing ifconfig");
                    return false;
                }
                Console.WriteLine("ifconfig written");
                return true;
            }
        }

        private static string superconscript()
        {
            return sudo.Properties.Resources.supercscript;
        }

        private static bool writesuperc()
        {
            string windir = System.Environment.GetEnvironmentVariable("windir");
            System.IO.FileInfo fileWrite = new System.IO.FileInfo(windir + "\\system32\\superc.cmd");
            if (fileWrite.Exists)
            {
                Console.WriteLine("File superc.cmd exists, skipping");
                return true;
            }
            else
            {
                try
                {
                    System.IO.StreamWriter writer = fileWrite.CreateText();
                    writer.WriteLine(superconscript());
                    writer.Flush();
                    writer.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error writing superc");
                    return false;
                }
                Console.WriteLine("superc written");
                return true;
            }
        }
    }
}
