﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Text;
using LANChat_Core;
using Shared;

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
            while (console() > 0) ;
        }

        private static Thread listenerTrd;

        private static int console()
        {
            Console.Write(">");
            string command = Console.ReadLine();
            string[] commandComps = command.Split(' ');

            switch (commandComps[0])
            {
                case "help":
                    Console.WriteLine("Available commands:\nstart {port} {max connections}\tStarts the server listening on that port and limits the users.\nstop\tStops the server.\nadd-user {username} {password}\t" + 
                        "Adds an user to the database.\ndel-user {username}\tDeletes the specified user.\nchange-pwd {username} {new password}\tChanges the password of an existing user.\n" +
                        "users-list\tReturns a list of all the users.\nbuffer-size [{new size}]\tChanges the size of the buffer to the new size, or returns the current buffer size\nexit\t Quit the application.");
                    console();
                    break;

                case "start":
                    try
                    {
                        if (commandComps.Length > 2)
                        {
                            int port = Convert.ToInt16(commandComps[1]);

                            if (Convert.ToInt16(commandComps[2]) < 1 || port < 1)
                            {
                                Utils.WriteColour("The port and the users limit must be in range 1-65535", ConsoleColor.Yellow);
                                break;
                            }

                            listenerTrd = new Thread(() => Server.Start(port, Convert.ToInt16(commandComps[2])));
                            listenerTrd.Start();
                            Utils.WriteColour("Server started on port " + port + ". Max users allowed: " + commandComps[2], ConsoleColor.Green); //TODO prevent to start it twice
                        }
                        else
                            Utils.WriteColour("Missing the port or the maximum connections allowed parameter!", ConsoleColor.Yellow);
                    }
                    catch (OverflowException e)
                    {
                        Utils.WriteColour("Overflow exception occurred. Port and users must be in range 1-65535", ConsoleColor.Red);
                    }
                    break;

                case "stop":
                    listenerTrd.Abort();
                    Utils.WriteColour("Server stopped", ConsoleColor.Green); //TODO
                    break;

                case "add-user":
                    if (commandComps.Length > 2)
                    {
                        int rowsUpdated = Database.ExecuteNonQuery(String.Format("INSERT INTO Users(Username, Password) VALUES('{0}', '{1}');", commandComps[1], commandComps[2]));
                        Utils.WriteColour(rowsUpdated + " rows updated", (rowsUpdated > 0 ? ConsoleColor.Green : ConsoleColor.Red));
                    }
                    else
                        Utils.WriteColour("Missing the username or the password!", ConsoleColor.Yellow);
                    break;

                case "del-user":
                    if (commandComps.Length > 1)
                    {
                        int rowsUpdated = Database.ExecuteNonQuery(String.Format("DELETE FROM Users WHERE Username='{0}';", command.Split(' ')[1]));
                        Utils.WriteColour(rowsUpdated + " rows updated", (rowsUpdated > 0 ? ConsoleColor.Green : ConsoleColor.Red));
                    }
                    else
                        Utils.WriteColour("Missing the name of the user to delete!", ConsoleColor.Yellow);
                    break;

                case "change-pwd":
                    if (commandComps.Length > 2)
                    {
                        int rowsUpdated = Database.ExecuteNonQuery(String.Format("UPDATE Users SET Password='{0}' WHERE Username='{1}';", commandComps[1], commandComps[2]));
                        Utils.WriteColour(rowsUpdated + " rows updated", (rowsUpdated > 0 ? ConsoleColor.Green : ConsoleColor.Red));
                    }
                    else
                        Utils.WriteColour("Missing the username or the new password!", ConsoleColor.Yellow);
                    break;

                case "users-list":
                    DataTable result = Database.ExecuteQuery("SELECT * FROM Users;");
                    Console.WriteLine("ID \t | Username \t | Password");
                    foreach (DataRow dataRow in result.Rows)
                    {
                        Console.WriteLine("{0} \t | {1} \t | {2}", dataRow.ItemArray[0], dataRow.ItemArray[1], dataRow.ItemArray[2]);
                    }
                    Utils.WriteColour(result.Rows.Count + " rows in database", ConsoleColor.Green);
                    break;

                case "buffer-size":
                    if (commandComps.Length == 1)
                        Console.WriteLine("The buffer size is currently " + LANChat_Core.Properties.Settings.Default.bufferSize + " bytes");
                    else
                    {
                        LANChat_Core.Properties.Settings.Default.bufferSize = Convert.ToInt32(commandComps[1]);
                        LANChat_Core.Properties.Settings.Default.Save();
                    }
                    break;

                case "exit":
                    return 0;
                default:
                    Utils.WriteColour("The command is not recognised, type help to get the list of supported commands!", ConsoleColor.Yellow);
                    break;
            }
            return 1;
        }
    }
}
