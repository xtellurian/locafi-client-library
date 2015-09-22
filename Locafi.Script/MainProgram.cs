using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Locafi.Client.Model.Query;
using Locafi.Client.Model.Query.PropertyComparison;
using Locafi.Client.UnitTests;

namespace Locafi.Script
{
    public class MainProgram
    {
        public async static void Entry(string userName, string password, Command command, string parameter)
        {



            await SetupAuth(userName, password);


            switch (command)
            {
                case Command.CleanUser:
                    if (parameter==null)
                    {
                        Console.WriteLine("Usage: <username> <password> <command> <user-to-clean>");
                        return;
                    }
                    await OnCleanCommand(userName, password, parameter);
                    break;
                default:
                    Console.WriteLine("Commands: 1. Clean\n");
                    break;
            }

            Thread.Sleep(1500);
        }

        private static async Task SetupAuth(string authUsername, string password)
        {
            var authRepo = WebRepoContainer.AuthRepo;
            var response = await authRepo.Login(authUsername, password);
            StringConstants.Token = response.TokenGroup.Token;
        }

        private static async Task OnCleanCommand(string authUsername, string password, string userToclean)
        {
            StringConstants.EmailAddress = authUsername;
            StringConstants.Password = password;

            var userRepo = WebRepoContainer.UserRepo;
            var userQuery = new UserQuery();
            userQuery.CreateQuery(U => U.UserName, userToclean, ComparisonOperator.Equals);
            var users = await userRepo.QueryUsers(userQuery);
            var userId = users.FirstOrDefault().Id;


            await Cleaner.CleanItems(userId);
            await Cleaner.CleanPlaces(userId);
        }
    }
}
