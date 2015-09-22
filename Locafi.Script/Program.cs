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
            if (args.Length < 3)
            {
                Console.WriteLine("Usage: <username> <password> <command>  <...Extra Params>");
                return;
            }
            var command = args[2];
            var userName = args[0];
            var password = args[1];
            Command realCommand;
            switch (command)
            {
                case "Clean":
                    realCommand = Command.CleanUser;
                    break;
                default:
                    Console.WriteLine("Unknown Command - " + command);
                    return;
            }

            MainProgram.Entry(userName, password, realCommand, args[3]);

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
        CleanUser
    }
}
