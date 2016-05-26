using System;
using Locafi.Client.Model.Dto.Items;
using Locafi.Client.Model.Query;
using Locafi.Client.Model.Query.Builder;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Locafi.Client.UnitTests.Tests.Rian
{
    [TestClass]
    public class QueryBuilderTests
    {
        [TestMethod]
        public void QueryBuilder_SimpleTest()
        {
            var query =
                QueryBuilder<ItemSummaryDto>.NewQuery(i => i.Name, "hello", ComparisonOperator.Equals)
                    .Or(i => i.TagNumber, "123", ComparisonOperator.Contains)
                    .Build();
            var value = query.AsRestQuery();
            Assert.IsInstanceOfType(query, typeof(IRestQuery<ItemSummaryDto>));
        }
    }
}
