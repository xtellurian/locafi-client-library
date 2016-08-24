using System.Linq;
using System.Threading.Tasks;
using Locafi.Client.Contract.Repo;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Locafi.Client.UnitTests.EntityGenerators;
using System.Collections.Generic;
using System;
using Locafi.Client.UnitTests.Validators;
using Locafi.Client.UnitTests.Implementations;
using Locafi.Client.Model.RFID;
using Locafi.Client.Processors.Encoding;
using Locafi.Client.Exceptions;

namespace Locafi.Client.UnitTests.Tests
{
    [TestClass]
    public class TagReservationRepoTests
    {
        private ITagReservationRepo _tagReservationRepo;
        private ISkuRepo _skuRepo;
        private IList<Guid> _skusToDelete;
        private static int _numTagsToReserve = 10;

        [TestInitialize]
        public void Initialise()
        {
            _tagReservationRepo = WebRepoContainer.TagReservationRepo;
            _skuRepo = WebRepoContainer.SkuRepo;
            _skusToDelete = new List<Guid>();
        }

        [TestCleanup]
        public void Cleanup()
        {
            // delete all skus that were created
            foreach (var Id in _skusToDelete)
            {
                _skuRepo.DeleteSku(Id).Wait();
            }
        }

        [TestMethod]
        public async Task TagReservations_ReserveTagForSgtin()
        {
            // create a sku
            var addDto = await SkuGenerator.GenerateSgtinSkuDto();
            var sku = await _skuRepo.CreateSku(addDto);
            _skusToDelete.Add(sku.Id);   // store to delete later

            // reserve tags for the sku
            var reservation = await _tagReservationRepo.ReserveTagsForSku(sku.Id, _numTagsToReserve);

            // check the response
            Validator.IsNotNull(reservation);
            Validator.IsTrue(reservation.TagNumbers.Count == _numTagsToReserve);
            Validator.IsFalse(string.IsNullOrEmpty(reservation.TagNumbers.First()));

            // ensure that the tag numbers generated match the sgtin
            var tag = new TestTag(reservation.TagNumbers.First());
            Validator.IsTrue(Sgtin.HasSgtin(tag));
            Validator.IsTrue(sku.CompanyPrefix.Equals(Sgtin.GetCompanyPrefix(tag)));
            Validator.IsTrue(sku.ItemReference.Equals(Sgtin.GetItemreference(tag)));
        }

        [TestMethod]
        public async Task TagReservations_ReserveTagForCustomPrefix()
        {
            // create a sku
            var addDto = await SkuGenerator.GenerateTagPrefixSkuDto();
            var sku = await _skuRepo.CreateSku(addDto);
            _skusToDelete.Add(sku.Id);   // store to delete later

            // reserve tags for the sku
            var reservation = await _tagReservationRepo.ReserveTagsForSku(sku.Id, _numTagsToReserve);

            // check the response
            Validator.IsNotNull(reservation);
            Validator.IsTrue(reservation.TagNumbers.Count == _numTagsToReserve);
            Validator.IsFalse(string.IsNullOrEmpty(reservation.TagNumbers.First()));

            // ensure that the tag number matches the custom prefix requirements
            var prefix = sku.CustomPrefix;
            var tagNumberPrefix = reservation.TagNumbers.First().Substring(0,prefix.Length);
            Validator.IsTrue(tagNumberPrefix.Equals(prefix));

        }

        // Cannot reserve a tag number for a sku with no custom prefix or gtin info, so this test is checking for failure
        [TestMethod]
        public async Task TagReservations_ReserveTagForBasicSku()
        {
            // create a sku
            var addDto = await SkuGenerator.GeneratePlainSkuDto();
            var sku = await _skuRepo.CreateSku(addDto);
            _skusToDelete.Add(sku.Id);   // store to delete later

            try {
                // reserve tags for the sku
                var reservation = await _tagReservationRepo.ReserveTagsForSku(sku.Id, _numTagsToReserve);

                // check the response
                Validator.IsNotNull(reservation);
                Validator.IsFalse(reservation.TagNumbers.Count == _numTagsToReserve);
                Validator.IsTrue(reservation.TagNumbers.Count == 0);
            }catch(TagReservationRepoException e)
            {
                // this is expected since we can't reserve tags for this sku type
            }
        }
    }
}
