using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Contract.Repo;
using Locafi.Client.Repo;

namespace Locafi.Client.UnitTests.Tests.Rian
{
    public abstract class WebRepoTestsBase
    {
        private IPlaceRepo _placeRepo;
        private IItemRepo _itemRepo;
        private IPersonRepo _personRepo;
        private ISkuRepo _skuRepo;
        private ISkuGroupRepo _skuGroupRepo;
        private IInventoryRepo _inventoryRepo;
        private ISnapshotRepo _snapshotRepo;
        private IReasonRepo _reasonRepo;
        private ITagReservationRepo _tagReservationRepo;

        protected IInventoryRepo InventoryRepo => _inventoryRepo ?? (_inventoryRepo = WebRepoContainer.InventoryRepo);
        protected IPlaceRepo PlaceRepo => _placeRepo ?? (_placeRepo = WebRepoContainer.PlaceRepo);
        protected IItemRepo ItemRepo => _itemRepo ?? (_itemRepo = WebRepoContainer.ItemRepo);
        protected IPersonRepo PersonRepo => _personRepo ?? (_personRepo = WebRepoContainer.PersonRepo);
        protected IReasonRepo ReasonRepo => _reasonRepo ?? (_reasonRepo = WebRepoContainer.ReasonRepo);
        protected ISkuRepo SkuRepo => _skuRepo ?? (_skuRepo = WebRepoContainer.SkuRepo);
        protected ISkuGroupRepo SkuGroupRepo => _skuGroupRepo ?? (_skuGroupRepo = WebRepoContainer.SkuGroupRepo);
        protected ISnapshotRepo SnapshotRepo => _snapshotRepo ?? (_snapshotRepo = WebRepoContainer.SnapshotRepo);
        protected ITagReservationRepo TagReservationRepo => _tagReservationRepo ?? (_tagReservationRepo = WebRepoContainer.TagReservationRepo);

        protected WebRepoTestsBase()
        {
            
        }

    }
}
