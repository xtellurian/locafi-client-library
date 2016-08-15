using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Locafi.Client.Model.Query;
using Locafi.Client.Model.Query.PropertyComparison;
using Locafi.Client.UnitTests;

namespace Locafi.Script
{
    class Program
    {
        private static bool _isRunning = true;
        static void Main(string[] args)
        {
            var command = args[0];
            Command realCommand;
            switch (command)
            {
                case "BuildDemo":
                    realCommand = Command.BuildDemo;
                    break;
                case "BuildDev":
                    realCommand = Command.BuildDev;
                    break;
                case "Clean":
                    realCommand = Command.CleanUser;
                    if (args.Length < 3)
                    {
                        Console.WriteLine("Usage: <command> <username> <password> <...Extra Params>");
                        return;
                    }
                    break;
                default:
                    Console.WriteLine("Unknown Command - " + command);
                    return;
            }

            var userName = args.Count() > 1 ? args[1] : "";
            var password = args.Count() > 2 ? args[2] : "";

            MainProgram.Entry(realCommand, userName, password, args.Count() > 3 ? args[3] : "");

            while (_isRunning)
            {
                
            }
        }

        public static void Exit()
        {
            _isRunning = false;
        }


    }
    public enum Command
    {
        CleanUser,
        BuildDev,
        BuildDemo
    }
}
