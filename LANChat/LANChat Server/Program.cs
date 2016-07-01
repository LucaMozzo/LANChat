using System;
using System.Collections.Generic;
using System.Data;
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

            Console.WriteLine("Enter command (type 'help' to show command list)");
            console();

            Console.ReadKey();
        }

        private static void console()
        {
            Console.Write(">");
            string command = Console.ReadLine();
            switch (command.Split(' ')[0])
            {
                case "help":
                    Console.WriteLine("Available commands:\nstart {port} {max connections}\tStarts the server listening on that port and limits the users\nadd-user {username} {password}\t" + 
                        "Adds an user to the database.\ndel-user {username}\tDeletes the specified user.\nchange-pwd {username} {new password}\tChanges the password of an existing user.\nusers-list\tReturns a list of all the users.");
                    console();
                    break;
                case "start":
                    
                    break;
                case "add-user":
                    Console.WriteLine(Database.ExecuteNonQuery("INSERT INTO Users(Username, Password) VALUES('test', 'testpwd');")+ " rows updated");
                    console();
                    break;
                case "del-user":
                    break;
                case "change-pwd":
                    break;
                case "users-list":
                    DataTable result = Database.ExecuteQuery("SELECT * FROM Users;");
                    Console.WriteLine("ID\tUsername\tPassword");
                    foreach (DataRow dataRow in result.Rows)
                    {
                        foreach (var item in dataRow.ItemArray)
                        {
                            Console.Write(item + "\t");
                        }
                        Console.WriteLine();
                    }
                    Console.WriteLine(result.Rows.Count + " rows");
                    break;
                default:
                    break;
            }
        }
    }
}
