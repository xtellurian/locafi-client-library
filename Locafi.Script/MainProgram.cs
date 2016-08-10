using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Locafi.Client.Model.Query;
using Locafi.Client.Model.Query.PropertyComparison;
using Locafi.Client.UnitTests;
using Locafi.Client.Model.Dto.Authentication;
using Locafi.Builder;
using Locafi.Client.Model.Query.Builder;
using Locafi.Client.Model.Dto.Items;
using Locafi.Client.Model.Dto.Skus;
using Locafi.Client.Model.Dto.Templates;
using Locafi.Client.Model.Enums;
using Locafi.Client.Model.Dto.ExtendedProperties;
using Locafi.Client.Model.Dto.Roles;
using Locafi.Client.Model.Dto.Users;
using Locafi.Client.Model.Dto.Persons;
using Locafi.Client.Model.Dto.Places;

namespace Locafi.Script
{
    public class MainProgram
    {
        public async static void Entry(Command command, string userName, string password, string parameter)
        {
            switch (command)
            {
                case Command.BuildDemo:
                    await OnBuildDemoCommand();
                    break;
                case Command.BuildDev:
                    await OnBuildDevCommand();
                    break;
                case Command.CleanUser:
                    if (parameter==null)
                    {
                        Console.WriteLine("Usage: <username> <password> <command> <user-to-clean>");
                        return;
                    }
                    await SetupAuth(userName, password);
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

        private static async Task OnBuildDevCommand()
        {
            // try to log in with the dev user
            var authRepo = WebRepoContainer.AuthRepo;
            var authResponse = await authRepo.Login(DevEnvironment.RegUserEmail, DevEnvironment.RegUserPassword);
            // if success then continue
            if (authResponse.Success)
            {
                Console.WriteLine("Logged in");
                // store token
                StringConstants.Token = authResponse.TokenGroup.Token;
                await WebRepoContainer.AuthorisedHttpTransferConfigService.SetTokenGroupAsync(authResponse.TokenGroup);

                //await Cleaner.CleanItems(QueryBuilder<ItemSummaryDto>.NewQuery(i => i.SkuId,new Guid("4cdb3408-695a-4d61-b743-5282815d7dd7"),ComparisonOperator.Equals)
                //    .Or(i => i.SkuId, new Guid("8f04554b-6aa4-4b1a-a908-e2d7642955f8"), ComparisonOperator.Equals)
                //    .TakeAll().Build());

                // now initialise the db (get rid of/rename default templates)
                await InitialiseDb();
            }
            else
            {
                // need to register and create the system
                var registerDto = new RegistrationDto() {
                    OrganisationName = DevEnvironment.OrganisationName,
                    UserFirstName = DevEnvironment.UserFirstName,
                    UserLastName = DevEnvironment.UserLastName,
                    UserEmail = DevEnvironment.RegUserEmail,
                    Password = DevEnvironment.RegUserPassword
                };

                if(await authRepo.Register(registerDto))
                {
                    Console.WriteLine("Organisation Created");

                    authResponse = await authRepo.Login(DevEnvironment.RegUserEmail, DevEnvironment.RegUserPassword);
                    if (!authResponse.Success)
                    {
                        Console.WriteLine("Couldn't login");
                        return; // exit
                    }

                    Console.WriteLine("Logged in");
                    // store token
                    StringConstants.Token = authResponse.TokenGroup.Token;
                    await WebRepoContainer.AuthorisedHttpTransferConfigService.SetTokenGroupAsync(authResponse.TokenGroup);

                    // now initialise the db (get rid of/rename default templates)
                    await InitialiseDb();
                }
            }

            Console.WriteLine("Building Extended Properties.");
            // build the extended properties
            var extProps = await DevBuilder.BuildExtendedProperties(WebRepoContainer.ExtendedPropertyRepo);
            if (extProps != null)
                Console.WriteLine("Extended Properties built.");
            else
            {
                Console.WriteLine("Error building Extended Properties");
                return;
            }

            Console.WriteLine("Building Templates.");
            // build the Templates
            var templates = await DevBuilder.BuildTemplates(WebRepoContainer.TemplateRepo, extProps);
            if (templates != null)
            {
                Console.WriteLine("Templates built.");

                // add the place template to our list
                var tempSummary = (await WebRepoContainer.TemplateRepo.QueryTemplates(QueryBuilder<TemplateSummaryDto>.NewQuery(t => t.Name, DevEnvironment.PersonTemplateName, ComparisonOperator.Equals).Build())).Items.FirstOrDefault();
                var tempDetail = await WebRepoContainer.TemplateRepo.GetById(tempSummary.Id);
                templates.Add(tempDetail);
            }
            else
            {
                Console.WriteLine("Error building Templates");
                return;
            }

            Console.WriteLine("Building Skus.");
            // build the Skus
            var skus = await DevBuilder.BuildSkus(WebRepoContainer.SkuRepo, extProps, templates);
            if (skus != null)
                Console.WriteLine("Skus built.");
            else
            {
                Console.WriteLine("Error building Skus");
                return;
            }

            Console.WriteLine("Building Places.");
            // build the Places
            var places = await DevBuilder.BuildPlaces(WebRepoContainer.PlaceRepo, extProps, templates);
            if (places != null)
                Console.WriteLine("Places built.");
            else
            {
                Console.WriteLine("Error building Places");
                return;
            }

            Console.WriteLine("Building Persons.");
            // build the Persons
            var persons = await DevBuilder.BuildPersons(WebRepoContainer.PersonRepo, extProps, templates);
            if (persons != null)
                Console.WriteLine("Persons built.");
            else
            {
                Console.WriteLine("Error building Persons");
                return;
            }

            Console.WriteLine("Building Users.");
            var roles = (await WebRepoContainer.RoleRepo.QueryRoles(QueryBuilder<RoleSummaryDto>.NewQuery().TakeAll().Build())).Items.ToList();
            // build the Users
            var users = await DevBuilder.BuildUsers(WebRepoContainer.UserRepo, WebRepoContainer.PersonRepo, extProps, templates, roles);
            if (users != null)
                Console.WriteLine("Users built.");
            else
            {
                Console.WriteLine("Error building Users");
                return;
            }

            Console.WriteLine("Building Items.");
            // build the Items
            var items = await DevBuilder.BuildItems(WebRepoContainer.ItemRepo, extProps, skus, persons, places);
            if (items != null)
                Console.WriteLine("Items built.");
            else
            {
                Console.WriteLine("Error building Items");
                return;
            }
        }

        private static async Task OnBuildDemoCommand()
        {
        }

        private static async Task OnCleanCommand(string authUsername, string password, string userToclean)
        {
            StringConstants.EmailAddress = authUsername;
            StringConstants.Password = password;

            var userRepo = WebRepoContainer.UserRepo;
            var userQuery = new UserQuery();
            userQuery.CreateQuery(U => U.Email, userToclean, ComparisonOperator.Equals);
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

        private static async Task InitialiseDb()
        {
            // now initialise the db (get rid of/rename default templates)
            await Cleaner.CleanItems(QueryBuilder<ItemSummaryDto>.NewQuery().TakeAll().Build());
            await Cleaner.CleanSkus(QueryBuilder<SkuSummaryDto>.NewQuery().TakeAll().Build());
            await Cleaner.CleanTemplates(QueryBuilder<TemplateSummaryDto>.NewQuery(t => t.TemplateType, TemplateFor.Person, ComparisonOperator.NotEquals).TakeAll().Build());
            await Cleaner.CleanExtendedProperties(QueryBuilder<ExtendedPropertySummaryDto>.NewQuery().TakeAll().Build());
            var ptemplate = (await WebRepoContainer.TemplateRepo.GetTemplatesForType(TemplateFor.Person)).Items.FirstOrDefault();
            if (ptemplate != null)
                await WebRepoContainer.TemplateRepo.UpdateTemplate(new UpdateTemplateDto() { Id = ptemplate.Id, Name = DevEnvironment.PersonTemplateName });
        }

        private static async Task CleanDevDb()
        {
            // now clean the dev db (get rid of/rename default templates)
            await Cleaner.CleanItems(QueryBuilder<ItemSummaryDto>.NewQuery().TakeAll().Build());
            await Cleaner.CleanUsers(QueryBuilder<UserSummaryDto>.NewQuery(u => u.Email, DevEnvironment.RegUserEmail, ComparisonOperator.NotEquals).TakeAll().Build());
            await Cleaner.CleanPersons(QueryBuilder<PersonSummaryDto>.NewQuery(p => p.Email, DevEnvironment.RegUserEmail, ComparisonOperator.NotEquals).TakeAll().Build());
            await Cleaner.CleanPlaces(QueryBuilder<PlaceSummaryDto>.NewQuery().TakeAll().Build());
            await Cleaner.CleanSkus(QueryBuilder<SkuSummaryDto>.NewQuery().TakeAll().Build());
            await Cleaner.CleanTemplates(QueryBuilder<TemplateSummaryDto>.NewQuery(t => t.Name, DevEnvironment.PersonTemplateName, ComparisonOperator.NotEquals).TakeAll().Build());
            await Cleaner.CleanExtendedProperties(QueryBuilder<ExtendedPropertySummaryDto>.NewQuery(p => p.TemplateType, TemplateFor.Person,ComparisonOperator.NotEquals).TakeAll().Build());
        }
    }
}
