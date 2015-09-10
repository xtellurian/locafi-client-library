using System.Linq;
using System.Threading.Tasks;
using Locafi.Client.Contract.Repo;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Locafi.Client.UnitTests.Tests.Rian
{
    [TestClass]
    public class TagReservationRepoTests
    {
        private ITagReservationRepo _tagReservationRepo;
        private ISkuRepo _skuRepo;

        [TestInitialize]
        public void Initialise()
        {
            _tagReservationRepo = WebRepoContainer.TagReservationRepo;
            _skuRepo = WebRepoContainer.SkuRepo;
        }

  //      [TestMethod]
        public async Task TagReservations_Reserve1ForEverySku()
        {
            var num = 1;
            var skus = await _skuRepo.GetAllSkus();
            Assert.IsNotNull(skus);
            Assert.IsTrue(skus.Count > 0);
            foreach (var sku in skus)
            {
                var reservation = await _tagReservationRepo.ReserveTagsForSku(sku.Id, num);
                Assert.IsNotNull(reservation);
                Assert.IsTrue(reservation.TagNumbers.Count == num);
                Assert.IsFalse(string.IsNullOrEmpty(reservation.TagNumbers.First()));
            }
        }
    }
}
