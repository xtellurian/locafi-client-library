using Locafi.Client.Contract.Repo;
using Locafi.Client.Exceptions;
using Locafi.Client.Model.Dto.Items;
using Locafi.Client.Model.Dto.Skus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.UnitTests.EntityGenerators
{
    public static class ItemGenerator
    {
        public static async Task<List<ItemDetailDto>> GenerateItems(Dictionary<Guid,int> itemCategories, Guid? itemPlaceId = null)
        {
            ITagReservationRepo _tagReservationRepo = WebRepoContainer.TagReservationRepo;
            IPlaceRepo _placeRepo = WebRepoContainer.PlaceRepo;
            IPersonRepo _personRepo = WebRepoContainer.PersonRepo;
            ISkuRepo _skuRepo = WebRepoContainer.SkuRepo;
            IItemRepo _itemRepo = WebRepoContainer.ItemRepo;

            List<ItemDetailDto> createdItems = new List<ItemDetailDto>();
            Guid placeId;

            var ran = new Random(DateTime.UtcNow.Millisecond);

            if (!itemPlaceId.HasValue)
            {
                // choose a place
                var places = await _placeRepo.QueryPlaces();
                var place = places.Items.ElementAt(ran.Next(places.Items.Count())); // picks a random place for the item
                placeId = place.Id;
            } else
                placeId = itemPlaceId.Value;

            // choose a person
            var persons = await _personRepo.QueryPersons();
            var person = persons.Items.ElementAt(ran.Next(persons.Items.Count()));

            if (itemCategories != null)
            {
                foreach (var category in itemCategories)
                {
                    SkuDetailDto skuDetail = await _skuRepo.GetSku(category.Key);

                    try
                    {
                        // try and get tag numbers for this sku
                        var tagNumbers = await _tagReservationRepo.ReserveTagsForSku(category.Key, category.Value);

                        foreach (var tag in tagNumbers.TagNumbers)
                        {
                            var name = "Random - " + skuDetail.Name + " " + ran.Next().ToString();
                            var description = name + " - Description";
                            var addDto = new AddItemDto(skuDetail, placeId, name, description, tagNumber: tag);
                            createdItems.Add(await _itemRepo.CreateItem(addDto));
                        }
                    }
                    catch (TagReservationRepoException e)
                    {
                        // not able to generate tag numbers for this sku so create them manually
                        for (var i = 0; i < category.Value; i++)
                        {
                            var name = "Random - " + skuDetail.Name + " " + ran.Next().ToString();
                            var description = name + " - Description";
                            var addDto = new AddItemDto(skuDetail, placeId, name, description, tagNumber: Guid.NewGuid().ToString());
                            createdItems.Add(await _itemRepo.CreateItem(addDto));
                        }
                    }
                }
            }

            return createdItems;
        }
    }
}
