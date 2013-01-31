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
                        //these could be function but in a rush
                        //ls
                        string cmd = "dir %1";
                        string windir = System.Environment.GetEnvironmentVariable("windir");
                        System.IO.FileInfo fileWrite = new System.IO.FileInfo(windir + "\\system32\\ls.cmd");
                        if (fileWrite.Exists)
                        {
                            Console.WriteLine("File ls.cmd exists, skipping");
                        }
                        else
                        {
                            bool errored = false;
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
                                errored = true;
                            }
                            if (!errored)
                            {
                                Console.WriteLine("ls written");
                            }
                        }

                        //ifconfig
                        cmd = "ipconfig %1";
                        fileWrite = new System.IO.FileInfo(windir + "\\system32\\ifconfig.cmd");
                        if (fileWrite.Exists)
                        {
                            Console.WriteLine("File ifconfig.cmd exists, skipping");
                        }
                        else
                        {
                            bool errored = false;
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
                                errored = true;
                            }
                            if (!errored)
                            {
                                Console.WriteLine("ifconfig written");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("This command needs to be run from a administrative mode");
                    }
                }else{
                    if(args[0] == "/?")
                    {
                        System.Console.WriteLine("Win Sudo - Dan Berkowitz, https://github.com/daberkow/win_sudo");
                        System.Console.WriteLine("Run applications in Windows command prompt as administrator");
                        System.Console.WriteLine("");
                        System.Console.WriteLine("Usage: sudo [program] [/write]");
                        System.Console.WriteLine("");
                        System.Console.WriteLine("/write option (requires admin) writes ls and ifconfig to cmd");
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
    }
}
