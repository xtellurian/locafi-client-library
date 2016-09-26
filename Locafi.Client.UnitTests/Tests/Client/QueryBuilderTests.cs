using System;
using Locafi.Client.Model.Dto.Items;
using Locafi.Client.Model.Query;
using Locafi.Client.Model.Query.Builder;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Locafi.Client.Model.Enums;
using Locafi.Client.Contract.Repo;
using Locafi.Client.Model;
using System.Threading.Tasks;

namespace Locafi.Client.UnitTests.Tests
{
    [TestClass]
    public class QueryBuilderTests
    {
        private IItemRepo _itemRepo;

        [TestInitialize]
        public void Setup()
        {
            _itemRepo = WebRepoContainer.ItemRepo;
        }

        [TestCleanup]
        public void Cleanup()
        {
        }

        [TestMethod]
        public async Task QueryBuilder_SimpleTest()
        {
            var query =
                QueryBuilder<ItemSummaryDto>.NewQuery(i => i.Name, "hello", ComparisonOperator.Equals)
                    .Or(i => i.TagNumber, "123", ComparisonOperator.Contains)
                    .Build();
            var value = query.AsRestQuery();
            Assert.IsInstanceOfType(query, typeof(IRestQuery<ItemSummaryDto>));

            query =
                QueryBuilder<ItemSummaryDto>.NewQuery(i => i.PlaceId,WebRepoContainer.Place1Id,ComparisonOperator.Equals)
                    .AndOpenGroup()
                    .Or(i => i.State, ItemStateType.Present, ComparisonOperator.Equals)
                    .Or(i => i.State, ItemStateType.Moved, ComparisonOperator.Equals)
                    .Or(i => i.State, ItemStateType.Missing, ComparisonOperator.Equals)
                    .Or(i => i.State, ItemStateType.Cleared, ComparisonOperator.Equals)
                    .Or(i => i.State, ItemStateType.Received, ComparisonOperator.Equals)
                    .CloseGroup()
                    .Build();
            var result = await _itemRepo.QueryItems(query);
            Assert.IsInstanceOfType(result, typeof(PageResult<ItemSummaryDto>));

            query =
                QueryBuilder<ItemSummaryDto>.NewQuery(i => i.PlaceId, WebRepoContainer.Place1Id, ComparisonOperator.Equals)
                    .AndOpenGroup()
                    .Or(i => i.State, ItemStateType.Missing, ComparisonOperator.Equals)
                    .Or(i => i.State, ItemStateType.Cleared, ComparisonOperator.Equals)
                    .CloseGroup()
                    .Build();
            result = await _itemRepo.QueryItems(query);
            Assert.IsInstanceOfType(result, typeof(PageResult<ItemSummaryDto>));
        }
    }
}
