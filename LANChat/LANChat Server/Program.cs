using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace LANChat_Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            Console.Title = fileVersionInfo.ProductName;
            Console.WriteLine(fileVersionInfo.ProductName + " " + fileVersionInfo.ProductVersion + "\nCopyright (c) 2016 Luca Mozzo.\n");

            Console.WriteLine("Server is starting...");

            Console.ReadKey();
        }
    }
}
