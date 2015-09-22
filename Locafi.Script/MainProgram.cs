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


            Console.WriteLine("Press Enter key to exit");

             var line = Console.ReadLine();

            Program.Exit();

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
            var user = users.FirstOrDefault();
            if (user == null)
            {

                return;
            }
            var userId = user.Id;
            // clean items
            var itemQuery = ItemQuery.NewQuery(i=>i.CreatedByUserId, userId, ComparisonOperator.Equals);
            await Cleaner.CleanItems(itemQuery);

            // clean places
            var placeQuery = PlaceQuery.NewQuery(p=>p.CreatedByUserId, userId, ComparisonOperator.Equals);
            await Cleaner.CleanPlaces(placeQuery);

            // clean orders
            var orderQuery = OrderQuery.NewQuery(o => o.CreatedByUserId, userId, ComparisonOperator.Equals);
            await Cleaner.CleanOrders(orderQuery);

            // clean inventories
            var inventoryQuery = InventoryQuery.NewQuery(i => i.CreatedByUserId, userId, ComparisonOperator.Equals);
            await Cleaner.CleanInventories(inventoryQuery);

            // clean snapshots
            var snapshotQuery = SnapshotQuery.NewQuery(s => s.CreatedByUserId, userId, ComparisonOperator.Equals);
            await Cleaner.CleanSnapshots(snapshotQuery);

           

        }
    }
}
