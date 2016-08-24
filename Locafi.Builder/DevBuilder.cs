using Locafi.Builder.Model.Items;
using Locafi.Builder.Model.Persons;
using Locafi.Builder.Model.Places;
using Locafi.Builder.Model.Skus;
using Locafi.Builder.Model.Templates;
using Locafi.Builder.Model.Users;
using Locafi.Client.Contract.Repo;
using Locafi.Client.Model.Dto;
using Locafi.Client.Model.Dto.ExtendedProperties;
using Locafi.Client.Model.Dto.Items;
using Locafi.Client.Model.Dto.Persons;
using Locafi.Client.Model.Dto.Places;
using Locafi.Client.Model.Dto.Roles;
using Locafi.Client.Model.Dto.Skus;
using Locafi.Client.Model.Dto.Tags;
using Locafi.Client.Model.Dto.Templates;
using Locafi.Client.Model.Dto.Users;
using Locafi.Client.Model.Enums;
using Locafi.Client.Model.Query;
using Locafi.Client.Model.Query.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Builder
{
    public static class DevBuilder
    {
        public static async Task<IList<ExtendedPropertyDetailDto>> BuildExtendedProperties(IExtendedPropertyRepo repo)
        {
            try
            {
                var extPropList = new List<ExtendedPropertyDetailDto>();

                // loop through and create all extended properties
                foreach (var addDto in DevEnvironment.ExtendedPropertiesToCreate)
                {
                    // check exists
                    var query = QueryBuilder<ExtendedPropertySummaryDto>.NewQuery(e => e.Name, addDto.Name, ComparisonOperator.Equals)
                        .And(e => e.TemplateType, addDto.TemplateType, ComparisonOperator.Equals)
                        .And(e => e.DataType, addDto.DataType, ComparisonOperator.Equals)
                        .Build();
                    var prop = await repo.QueryExtendedProperties(query);
                    if (prop.Items.Count() > 0)
                    {
                        // add existing to return list
                        extPropList.Add(await repo.GetExtendedPropertyById(prop.Items.First().Id));
                    }
                    else
                    {
                        // create
                        var detailDto = await repo.CreateExtendedProperty(addDto);
                        // add to return list
                        extPropList.Add(detailDto);
                    }
                }

                return extPropList;
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }

            return null;            
        }

        public static async Task<IList<TemplateDetailDto>> BuildTemplates(ITemplateRepo repo, IList<ExtendedPropertyDetailDto> extProps)
        {
            try
            {
                var templateList = new List<TemplateDetailDto>();

                // loop through and create all extended properties
                foreach (var addDto in DevEnvironment.TemplatesToCreate)
                {
                    // check exists
                    var query = QueryBuilder<TemplateSummaryDto>.NewQuery(e => e.Name, addDto.Name, ComparisonOperator.Equals)
                        .And(e => e.TemplateType, addDto.TemplateType, ComparisonOperator.Equals)
                        .Build();
                    var temp = await repo.QueryTemplates(query);
                    if (temp.Items.Count() > 0)
                    {
                        TemplateDetailDto detailDto = null;
                        // get template
                        var template = await repo.GetById(temp.Items.First().Id);
                        // check that it has all the required extended properties
                        if (template.TemplateExtendedPropertyList.Count() != addDto.BuilderTemplateExtendedPropertyList.Count())
                        {
                            // don't match so need to update
                            var updateDto = BuildUpdateTemplateDto(template.Id, addDto, extProps);
                            // add to return list
                            detailDto = await repo.UpdateTemplate(updateDto);
                        }
                        else if(template.TemplateExtendedPropertyList.Count() == 0)
                        {
                            // equal with no extended properties so add existing to return list
                            detailDto = template;
                        }else
                        {
                            detailDto = template;
                            // equal numbers so need to check that they are all the same
                            foreach (var prop in template.TemplateExtendedPropertyList)
                            {
                                var bprop = addDto.BuilderTemplateExtendedPropertyList.FirstOrDefault(p => p.ExtendedPropertyName == prop.ExtendedPropertyName);
                                // have a miss match
                                if(bprop == null)
                                {
                                    // don't match so need to update
                                    var updateDto = BuildUpdateTemplateDto(template.Id, addDto, extProps);
                                    // add to return list
                                    detailDto = await repo.UpdateTemplate(updateDto);
                                    break;
                                }
                            }
                        }

                        // add to return list
                        templateList.Add(detailDto);
                    }
                    else
                    {
                        // create
                        var detailDto = await repo.CreateTemplate(BuildAddTemplateDto(addDto, extProps));
                        // add to return list
                        templateList.Add(detailDto);
                    }
                }

                return templateList;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return null;
        }

        public static async Task<IList<SkuDetailDto>> BuildSkus(ISkuRepo repo, IList<ExtendedPropertyDetailDto> extProps, IList<TemplateDetailDto> templates)
        {
            try
            {
                var categoryList = new List<SkuDetailDto>();

                // loop through and create all extended properties
                foreach (var addDto in DevEnvironment.SkusToCreate)
                {
                    // check exists
                    var query = QueryBuilder<SkuSummaryDto>.NewQuery(e => e.Name, addDto.Name, ComparisonOperator.Equals)
                        .And(e => e.SkuNumber, addDto.SkuNumber, ComparisonOperator.Equals)
                        .And(e => e.CompanyPrefix, addDto.CompanyPrefix, ComparisonOperator.Equals)
                        .And(e => e.ItemReference, addDto.ItemReference, ComparisonOperator.Equals)
                        .And(e => e.CustomPrefix, addDto.CustomPrefix, ComparisonOperator.Equals)
                        .And(e => e.TemplateName, addDto.TemplateName, ComparisonOperator.Equals)
                        .Build();
                    var cat = await repo.QuerySkus(query);
                    if (cat.Items.Count() > 0)
                    {
                        SkuDetailDto detailDto = null;
                        // get template
                        var category = await repo.GetSku(cat.Items.First().Id);
                        // check that it has all the required extended properties
                        if (category.SkuExtendedPropertyList.Count() != addDto.BuilderSkuExtendedPropertyList.Count())
                        {
                            // don't match so need to update
                            var updateDto = BuildUpdateSkuDto(category.Id, addDto, extProps, templates);
                            // add to return list
                            detailDto = await repo.UpdateSku(updateDto);
                        }
                        else if (category.SkuExtendedPropertyList.Count() == 0)
                        {
                            // equal with no extended properties so add existing to return list
                            detailDto = category;
                        }
                        else
                        {
                            detailDto = category;
                            // equal numbers so need to check that they are all the same
                            foreach (var prop in category.SkuExtendedPropertyList)
                            {
                                var bprop = addDto.BuilderSkuExtendedPropertyList.FirstOrDefault(p => p.ExtendedPropertyName == prop.ExtendedPropertyName);
                                // have a miss match
                                if (bprop == null)
                                {
                                    // don't match so need to update
                                    var updateDto = BuildUpdateSkuDto(category.Id, addDto, extProps, templates);
                                    // add to return list
                                    detailDto = await repo.UpdateSku(updateDto);
                                    break;
                                }
                            }
                        }

                        // add to return list
                        categoryList.Add(detailDto);
                    }
                    else
                    {
                        // create
                        var detailDto = await repo.CreateSku(BuildAddSkuDto(addDto, extProps, templates));
                        // add to return list
                        categoryList.Add(detailDto);
                    }
                }

                return categoryList;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return null;
        }

        public static async Task<IList<PlaceDetailDto>> BuildPlaces(IPlaceRepo repo, IList<ExtendedPropertyDetailDto> extProps, IList<TemplateDetailDto> templates)
        {
            try
            {
                var placeList = new List<PlaceDetailDto>();

                // loop through and create all extended properties
                foreach (var addDto in DevEnvironment.PlacesToCreate)
                {
                    // check exists
                    var query = QueryBuilder<PlaceSummaryDto>.NewQuery(e => e.Name, addDto.Name, ComparisonOperator.Equals)
                        .Build();
                    var placeQuery = await repo.QueryPlaces(query);
                    if (placeQuery.Items.Count() > 0)
                    {
                        PlaceDetailDto detailDto = null;
                        // get place
                        var place = await repo.GetPlaceById(placeQuery.Items.First().Id);

                        // check that it has all the required extended properties
                        if (place.PlaceExtendedPropertyList.Count() != addDto.BuilderPlaceExtendedPropertyList.Count())
                        {
                            // don't match so need to update
                            var updateDto = BuildUpdatePlaceDto(place.Id, addDto, extProps, templates);
                            // add to return list
                            detailDto = await repo.UpdatePlace(updateDto);
                        }
                        else if (place.PlaceExtendedPropertyList.Count() == 0)
                        {
                            // equal with no extended properties so add existing to return list
                            detailDto = place;
                        }
                        else
                        {
                            detailDto = place;
                            // equal numbers so need to check that they are all the same
                            foreach (var prop in place.PlaceExtendedPropertyList)
                            {
                                var bprop = addDto.BuilderPlaceExtendedPropertyList.FirstOrDefault(p => p.ExtendedPropertyName == prop.ExtendedPropertyName);
                                // have a miss match
                                if (bprop == null)
                                {
                                    // don't match so need to update
                                    var updateDto = BuildUpdatePlaceDto(place.Id, addDto, extProps, templates);
                                    // add to return list
                                    detailDto = await repo.UpdatePlace(updateDto);
                                    break;
                                }
                            }
                        }

                        // now check that all the tags are the same
                        if(detailDto.PlaceTagList.Count() != addDto.PlaceTagList.Count())
                        {
                            // different number so update
                            var updateDto = BuildUpdatePlaceTagDto(place.Id, addDto);
                            // add to return list
                            detailDto = await repo.UpdatePlaceTag(updateDto);
                        }
                        else if(detailDto.PlaceTagList.Count() == 0)
                        {
                            // they are the same so nothing more to do
                        }else
                        {
                            // equal number so check that all the tags are the same
                            foreach(var tag in detailDto.PlaceTagList)
                            {
                                var btag = addDto.PlaceTagList.FirstOrDefault(t => t.TagNumber == tag.TagNumber && t.TagType == tag.TagType);
                                if(btag != null)
                                {
                                    // don't match so need to update
                                    var updateDto = BuildUpdatePlaceTagDto(place.Id, addDto);
                                    // add to return list
                                    detailDto = await repo.UpdatePlaceTag(updateDto);
                                    break;
                                }
                            }
                        }

                        // add to return list
                        placeList.Add(detailDto);
                    }
                    else
                    {
                        // create
                        var detailDto = await repo.CreatePlace(BuildAddPlaceDto(addDto, extProps, templates));
                        // add to return list
                        placeList.Add(detailDto);
                    }
                }

                return placeList;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return null;
        }

        public static async Task<IList<PersonDetailDto>> BuildPersons(IPersonRepo repo, IList<ExtendedPropertyDetailDto> extProps, IList<TemplateDetailDto> templates)
        {
            try
            {
                var personList = new List<PersonDetailDto>();

                // loop through and create all extended properties
                foreach (var addDto in DevEnvironment.PersonsToCreate)
                {
                    // check exists
                    var query = QueryBuilder<PersonSummaryDto>.NewQuery(e => e.GivenName, addDto.GivenName, ComparisonOperator.Equals)
                        .And(e => e.Surname, addDto.Surname, ComparisonOperator.Equals)
                        .And(e => e.Email, addDto.Email, ComparisonOperator.Equals)
                        .Build();
                    var personQuery = await repo.QueryPersons(query);
                    if (personQuery.Items.Count() > 0)
                    {
                        PersonDetailDto detailDto = null;
                        // get person
                        var person = await repo.GetPersonById(personQuery.Items.First().Id);

                        // check that it has all the required extended properties
                        if (person.PersonExtendedPropertyList.Count() != addDto.BuilderPersonExtendedPropertyList.Count())
                        {
                            // don't match so need to update
                            var updateDto = BuildUpdatePersonDto(person.Id, addDto, extProps, templates);
                            // add to return list
                            detailDto = await repo.UpdatePerson(updateDto);
                        }
                        else if (person.PersonExtendedPropertyList.Count() == 0)
                        {
                            // equal with no extended properties so add existing to return list
                            detailDto = person;
                        }
                        else
                        {
                            detailDto = person;
                            // equal numbers so need to check that they are all the same
                            foreach (var prop in person.PersonExtendedPropertyList)
                            {
                                var bprop = addDto.BuilderPersonExtendedPropertyList.FirstOrDefault(p => p.ExtendedPropertyName == prop.ExtendedPropertyName);
                                // have a miss match
                                if (bprop == null)
                                {
                                    // don't match so need to update
                                    var updateDto = BuildUpdatePersonDto(person.Id, addDto, extProps, templates);
                                    // add to return list
                                    detailDto = await repo.UpdatePerson(updateDto);
                                    break;
                                }
                            }
                        }

                        // now check that all the tags are the same
                        if (detailDto.PersonTagList.Count() != addDto.PersonTagList.Count())
                        {
                            // different number so update
                            var updateDto = BuildUpdatePersonTagDto(person.Id, addDto);
                            // add to return list
                            detailDto = await repo.UpdatePersonTag(updateDto);
                        }
                        else if (detailDto.PersonTagList.Count() == 0)
                        {
                            // they are the same so nothing more to do
                        }
                        else
                        {
                            // equal number so check that all the tags are the same
                            foreach (var tag in detailDto.PersonTagList)
                            {
                                var btag = addDto.PersonTagList.FirstOrDefault(t => t.TagNumber == tag.TagNumber && t.TagType == tag.TagType);
                                if (btag != null)
                                {
                                    // don't match so need to update
                                    var updateDto = BuildUpdatePersonTagDto(person.Id, addDto);
                                    // add to return list
                                    detailDto = await repo.UpdatePersonTag(updateDto);
                                    break;
                                }
                            }
                        }

                        // add to return list
                        personList.Add(detailDto);
                    }
                    else
                    {
                        // create
                        var detailDto = await repo.CreatePerson(BuildAddPersonDto(addDto, extProps, templates));
                        // add to return list
                        personList.Add(detailDto);
                    }
                }

                return personList;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return null;
        }

        public static async Task<IList<UserDetailDto>> BuildUsers(IUserRepo repo, IPersonRepo personRepo, IList<ExtendedPropertyDetailDto> extProps, IList<TemplateDetailDto> templates, IList<RoleSummaryDto> roles)
        {
            try
            {
                var userList = new List<UserDetailDto>();

                // loop through and create all extended properties
                foreach (var addDto in DevEnvironment.UsersToCreate)
                {
                    // check exists
                    var query = QueryBuilder<UserSummaryDto>.NewQuery(e => e.GivenName, addDto.GivenName, ComparisonOperator.Equals)
                        .And(e => e.Surname, addDto.Surname, ComparisonOperator.Equals)
                        .And(e => e.Email, addDto.Email, ComparisonOperator.Equals)
                        .Build();
                    var userQuery = await repo.QueryUsers(query);
                    if (userQuery.Items.Count() > 0)
                    {
                        UserDetailDto detailDto = null;
                        // get user
                        var user = await repo.GetUserById(userQuery.Items.First().Id);

                        // check that it has all the required extended properties
                        if (user.PersonExtendedPropertyList.Count() != addDto.BuilderPersonExtendedPropertyList.Count())
                        {
                            // don't match so need to update
                            var updateDto = BuildUpdateUserDto(user.Id, addDto, extProps, templates, roles);
                            // add to return list
                            detailDto = await repo.UpdateUser(updateDto);
                        }
                        else if (user.PersonExtendedPropertyList.Count() == 0)
                        {
                            // equal with no extended properties so add existing to return list
                            detailDto = user;
                        }
                        else
                        {
                            detailDto = user;
                            // equal numbers so need to check that they are all the same
                            foreach (var prop in user.PersonExtendedPropertyList)
                            {
                                var bprop = addDto.BuilderPersonExtendedPropertyList.FirstOrDefault(p => p.ExtendedPropertyName == prop.ExtendedPropertyName);
                                // have a miss match
                                if (bprop == null)
                                {
                                    // don't match so need to update
                                    var updateDto = BuildUpdateUserDto(user.Id, addDto, extProps, templates, roles);
                                    // add to return list
                                    detailDto = await repo.UpdateUser(updateDto);
                                    break;
                                }
                            }
                        }

                        // now check that all the tags are the same
                        if (detailDto.PersonTagList.Count() != addDto.PersonTagList.Count())
                        {
                            // different number so update
                            var updateDto = BuildUpdatePersonTagDto(user.PersonId, addDto);
                            // update persons tags
                            await personRepo.UpdatePersonTag(updateDto);
                        }
                        else if (detailDto.PersonTagList.Count() == 0)
                        {
                            // they are the same so nothing more to do
                        }
                        else
                        {
                            // equal number so check that all the tags are the same
                            foreach (var tag in detailDto.PersonTagList)
                            {
                                var btag = addDto.PersonTagList.FirstOrDefault(t => t.TagNumber == tag.TagNumber && t.TagType == tag.TagType);
                                if (btag != null)
                                {
                                    // don't match so need to update
                                    var updateDto = BuildUpdatePersonTagDto(user.PersonId, addDto);
                                    // update persons tags
                                    await personRepo.UpdatePersonTag(updateDto);
                                    break;
                                }
                            }
                        }

                        // add to return list
                        userList.Add(detailDto);
                    }
                    else
                    {
                        // create
                        var detailDto = await repo.CreateUser(BuildAddUserDto(addDto, extProps, templates, roles));
                        // add to return list
                        userList.Add(detailDto);
                    }
                }

                return userList;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return null;
        }

        public static async Task<IList<ItemDetailDto>> BuildItems(IItemRepo repo, IList<ExtendedPropertyDetailDto> extProps, IList<SkuDetailDto> skus, IList<PersonDetailDto> persons, IList<PlaceDetailDto> places)
        {
            try
            {
                var itemList = new List<ItemDetailDto>();

                // loop through and create all extended properties
                foreach (var addDto in DevEnvironment.ItemsToCreate)
                {
                    // check exists
                    var query = QueryBuilder<ItemSummaryDto>.NewQuery(e => e.Name, addDto.Name, ComparisonOperator.Equals)
                        .And(e => e.TagNumber, addDto.ItemTagList[0].TagNumber, ComparisonOperator.Equals)
                        .Build();
                    var itemQuery = await repo.QueryItems(query);
                    if (itemQuery.Items.Count() > 0)
                    {
                        ItemDetailDto detailDto = null;
                        // get item
                        var item = await repo.GetItemDetail(itemQuery.Items.First().Id);

                        // check that it has all the required extended properties
                        if (item.ItemExtendedPropertyList.Count() != addDto.BuilderItemExtendedPropertyList.Count())
                        {
                            // don't match so need to update
                            var updateDto = BuildUpdateItemDto(item.Id, addDto, extProps, skus, persons);
                            // add to return list
                            detailDto = await repo.UpdateItem(updateDto);
                        }
                        else if (item.ItemExtendedPropertyList.Count() == 0)
                        {
                            // equal with no extended properties so add existing to return list
                            detailDto = item;
                        }
                        else
                        {
                            detailDto = item;
                            // equal numbers so need to check that they are all the same
                            foreach (var prop in item.ItemExtendedPropertyList)
                            {
                                var bprop = addDto.BuilderItemExtendedPropertyList.FirstOrDefault(p => p.ExtendedPropertyName == prop.Name);
                                // have a miss match
                                if (bprop == null)
                                {
                                    // don't match so need to update
                                    var updateDto = BuildUpdateItemDto(item.Id, addDto, extProps, skus, persons);
                                    // add to return list
                                    detailDto = await repo.UpdateItem(updateDto);
                                    break;
                                }
                            }
                        }

                        // now check that all the tags are the same
                        if (detailDto.ItemTagList.Count() != addDto.ItemTagList.Count())
                        {
                            // different number so update
                            var updateDto = BuildUpdateItemTagDto(item.Id, addDto);
                            // add to return list
                            detailDto = await repo.UpdateTag(updateDto);
                        }
                        else if (detailDto.ItemTagList.Count() == 0)
                        {
                            // they are the same so nothing more to do
                        }
                        else
                        {
                            // equal number so check that all the tags are the same
                            foreach (var tag in detailDto.ItemTagList)
                            {
                                var btag = addDto.ItemTagList.FirstOrDefault(t => t.TagNumber == tag.TagNumber && t.TagType == tag.TagType);
                                if (btag != null)
                                {
                                    // don't match so need to update
                                    var updateDto = BuildUpdateItemTagDto(item.Id, addDto);
                                    // add to return list
                                    detailDto = await repo.UpdateTag(updateDto);
                                    break;
                                }
                            }
                        }

                        // now check if the place has changed
                        if(detailDto.PlaceName != addDto.PlaceName)
                        {
                            // don't match so need to update
                            var updateDto = BuildUpdateItemPlaceDto(item.Id, addDto, places);
                            // add to return list
                            detailDto = await repo.UpdateItemPlace(updateDto);
                        }

                        // add to return list
                        itemList.Add(detailDto);
                    }
                    else
                    {
                        // create
                        var detailDto = await repo.CreateItem(BuildAddItemDto(addDto, extProps, skus, persons, places));
                        // add to return list
                        itemList.Add(detailDto);
                    }
                }

                return itemList;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return null;
        }

        #region Private_Helpers
        private static UpdateTemplateDto BuildUpdateTemplateDto(Guid templateId, BuilderAddTemplateDto addDto, IList<ExtendedPropertyDetailDto> extProps)
        {
            var updateDto = new UpdateTemplateDto()
            {
                Id = templateId,
                Name = addDto.Name
            };

            foreach(var bprop in addDto.BuilderTemplateExtendedPropertyList)
            {
                // find the required extended property
                var prop = extProps.FirstOrDefault(p => p.Name == bprop.ExtendedPropertyName && p.TemplateType == addDto.TemplateType);
                updateDto.TemplateExtendedPropertyList.Add(new AddTemplateExtendedPropertyDto()
                {
                    ExtendedPropertyId = prop.Id,
                    IsRequired = bprop.IsRequired
                });
            }

            return updateDto;
        }

        private static AddTemplateDto BuildAddTemplateDto(BuilderAddTemplateDto bAddDto, IList<ExtendedPropertyDetailDto> extProps)
        {
            var addTemplateDto = new AddTemplateDto()
            {
                TemplateType = bAddDto.TemplateType,
                Name = bAddDto.Name
            };

            foreach (var bprop in bAddDto.BuilderTemplateExtendedPropertyList)
            {
                // find the required extended property
                var prop = extProps.FirstOrDefault(p => p.Name == bprop.ExtendedPropertyName && p.TemplateType == bAddDto.TemplateType);
                addTemplateDto.TemplateExtendedPropertyList.Add(new AddTemplateExtendedPropertyDto()
                {
                    ExtendedPropertyId = prop.Id,
                    IsRequired = bprop.IsRequired
                });
            }

            return addTemplateDto;
        }

        private static UpdateSkuDto BuildUpdateSkuDto(Guid Id, BuilderAddSkuDto bAddDto, IList<ExtendedPropertyDetailDto> extProps, IList<TemplateDetailDto> templates)
        {
            var updateDto = new UpdateSkuDto()
            {
                Id = Id,
                Name = bAddDto.Name,
                SkuNumber = bAddDto.SkuNumber,
                CompanyPrefix = bAddDto.CompanyPrefix,
                ItemReference = bAddDto.ItemReference,
                CustomPrefix = bAddDto.CustomPrefix
            };

            // find the item template with the desired name
            var template = templates.FirstOrDefault(t => t.Name == bAddDto.TemplateName && t.TemplateType == TemplateFor.Item);
            if (template == null)
                throw new InvalidOperationException("Template not found for updating sku");
            updateDto.TemplateId = template.Id;

            foreach (var bprop in bAddDto.BuilderSkuExtendedPropertyList)
            {
                // find the required extended property
                var prop = extProps.FirstOrDefault(p => p.Name == bprop.ExtendedPropertyName && p.TemplateType == template.TemplateType);
                updateDto.SkuExtendedPropertyList.Add(new WriteSkuExtendedPropertyDto()
                {
                    ExtendedPropertyId = prop.Id,
                    IsSkuLevelProperty = bprop.IsSkuLevelProperty
                });
            }

            return updateDto;
        }

        private static AddSkuDto BuildAddSkuDto(BuilderAddSkuDto bAddDto, IList<ExtendedPropertyDetailDto> extProps, IList<TemplateDetailDto> templates)
        {
            var addSkuDto = new AddSkuDto()
            {
                Name = bAddDto.Name,
                SkuNumber = bAddDto.SkuNumber,
                CompanyPrefix = bAddDto.CompanyPrefix,
                ItemReference = bAddDto.ItemReference,
                CustomPrefix = bAddDto.CustomPrefix
            };

            // find the item template with the desired name
            var template = templates.FirstOrDefault(t => t.Name == bAddDto.TemplateName && t.TemplateType == TemplateFor.Item);
            if (template == null)
                throw new InvalidOperationException("Template not found for adding sku");
            addSkuDto.ItemTemplateId = template.Id;

            foreach (var bprop in bAddDto.BuilderSkuExtendedPropertyList)
            {
                // find the required extended property
                var prop = extProps.FirstOrDefault(p => p.Name == bprop.ExtendedPropertyName && p.TemplateType == template.TemplateType);
                addSkuDto.SkuExtendedPropertyList.Add(new WriteSkuExtendedPropertyDto()
                {
                    ExtendedPropertyId = prop.Id,
                    IsSkuLevelProperty = bprop.IsSkuLevelProperty
                });
            }

            return addSkuDto;
        }

        private static UpdatePlaceDto BuildUpdatePlaceDto(Guid Id, BuilderAddPlaceDto bAddDto, IList<ExtendedPropertyDetailDto> extProps, IList<TemplateDetailDto> templates)
        {
            var updateDto = new UpdatePlaceDto()
            {
                Id = Id,
                Name = bAddDto.Name,
                Description = bAddDto.Description
            };

            // find the item template with the desired name
            var template = templates.FirstOrDefault(t => t.Name == bAddDto.TemplateName && t.TemplateType == TemplateFor.Place);
            if (template == null)
                throw new InvalidOperationException("Template not found for updating place");
            updateDto.TemplateId = template.Id;

            foreach (var bprop in bAddDto.BuilderPlaceExtendedPropertyList)
            {
                // find the required extended property
                var prop = extProps.FirstOrDefault(p => p.Name == bprop.ExtendedPropertyName && p.TemplateType == template.TemplateType);
                updateDto.PlaceExtendedPropertyList.Add(new WriteEntityExtendedPropertyDto()
                {
                    ExtendedPropertyId = prop.Id,
                    Value = bprop.Value
                });
            }

            return updateDto;
        }

        private static UpdatePlaceTagDto BuildUpdatePlaceTagDto(Guid Id, BuilderAddPlaceDto bAddDto)
        {
            var updateDto = new UpdatePlaceTagDto()
            {
                Id = Id
            };

            foreach (var btag in bAddDto.PlaceTagList)
            {
                updateDto.PlaceTagList.Add(btag);
            }

            return updateDto;
        }

        private static AddPlaceDto BuildAddPlaceDto(BuilderAddPlaceDto bAddDto, IList<ExtendedPropertyDetailDto> extProps, IList<TemplateDetailDto> templates)
        {
            var addDto = new AddPlaceDto()
            {
                Name = bAddDto.Name,
                Description = bAddDto.Description
            };

            // find the item template with the desired name
            var template = templates.FirstOrDefault(t => t.Name == bAddDto.TemplateName && t.TemplateType == TemplateFor.Place);
            if (template == null)
                throw new InvalidOperationException("Template not found for adding place");
            addDto.TemplateId = template.Id;

            // add all extended properties
            foreach (var bprop in bAddDto.BuilderPlaceExtendedPropertyList)
            {
                // find the required extended property
                var prop = extProps.FirstOrDefault(p => p.Name == bprop.ExtendedPropertyName && p.TemplateType == template.TemplateType);
                addDto.PlaceExtendedPropertyList.Add(new WriteEntityExtendedPropertyDto()
                {
                    ExtendedPropertyId = prop.Id,
                    Value = bprop.Value
                });
            }

            // add all tags
            foreach(var tag in bAddDto.PlaceTagList)
            {
                addDto.PlaceTagList.Add(tag);
            }

            return addDto;
        }

        private static UpdatePersonDto BuildUpdatePersonDto(Guid Id, BuilderAddPersonDto bAddDto, IList<ExtendedPropertyDetailDto> extProps, IList<TemplateDetailDto> templates)
        {
            var updateDto = new UpdatePersonDto()
            {
                Id = Id,
                GivenName = bAddDto.GivenName,
                Surname = bAddDto.Surname,
                Email = bAddDto.Email
            };

            // find the item template with the desired name
            var template = templates.FirstOrDefault(t => t.Name == bAddDto.TemplateName && t.TemplateType == TemplateFor.Person);
            if (template == null)
                throw new InvalidOperationException("Template not found for updating person");
            updateDto.TemplateId = template.Id;

            foreach (var bprop in bAddDto.BuilderPersonExtendedPropertyList)
            {
                // find the required extended property
                var prop = extProps.FirstOrDefault(p => p.Name == bprop.ExtendedPropertyName && p.TemplateType == template.TemplateType);
                updateDto.PersonExtendedPropertyList.Add(new WriteEntityExtendedPropertyDto()
                {
                    ExtendedPropertyId = prop.Id,
                    Value = bprop.Value
                });
            }

            return updateDto;
        }

        private static UpdatePersonTagDto BuildUpdatePersonTagDto(Guid Id, BuilderAddPersonDto bAddDto)
        {
            var updateDto = new UpdatePersonTagDto()
            {
                Id = Id
            };

            foreach (var btag in bAddDto.PersonTagList)
            {
                updateDto.PersonTagList.Add(btag);
            }

            return updateDto;
        }

        private static AddPersonDto BuildAddPersonDto(BuilderAddPersonDto bAddDto, IList<ExtendedPropertyDetailDto> extProps, IList<TemplateDetailDto> templates)
        {
            var addDto = new AddPersonDto()
            {
                GivenName = bAddDto.GivenName,
                Surname = bAddDto.Surname,
                Email = bAddDto.Email
            };

            // find the item template with the desired name
            var template = templates.FirstOrDefault(t => t.Name == bAddDto.TemplateName && t.TemplateType == TemplateFor.Person);
            if (template == null)
                throw new InvalidOperationException("Template not found for adding person");
            addDto.TemplateId = template.Id;

            // add all extended properties
            foreach (var bprop in bAddDto.BuilderPersonExtendedPropertyList)
            {
                // find the required extended property
                var prop = extProps.FirstOrDefault(p => p.Name == bprop.ExtendedPropertyName && p.TemplateType == template.TemplateType);
                addDto.PersonExtendedPropertyList.Add(new WriteEntityExtendedPropertyDto()
                {
                    ExtendedPropertyId = prop.Id,
                    Value = bprop.Value
                });
            }

            // add all tags
            foreach (var tag in bAddDto.PersonTagList)
            {
                addDto.PersonTagList.Add(tag);
            }

            return addDto;
        }

        private static UpdateUserDto BuildUpdateUserDto(Guid Id, BuilderAddUserDto bAddDto, IList<ExtendedPropertyDetailDto> extProps, IList<TemplateDetailDto> templates, IList<RoleSummaryDto> roles)
        {
            var updateDto = new UpdateUserDto()
            {
                Id = Id,
                GivenName = bAddDto.GivenName,
                Surname = bAddDto.Surname,
                Email = bAddDto.Email
            };

            // find the item template with the desired name
            var template = templates.FirstOrDefault(t => t.Name == bAddDto.TemplateName && t.TemplateType == TemplateFor.Person);
            if (template == null)
                throw new InvalidOperationException("Template not found for updating user");
            updateDto.TemplateId = template.Id;

            // find the role with the desired name
            var role = roles.FirstOrDefault(t => t.Name == bAddDto.RoleName);
            if (role == null)
                throw new InvalidOperationException("Role not found for updating user");
            updateDto.RoleId = role.Id;

            foreach (var bprop in bAddDto.BuilderPersonExtendedPropertyList)
            {
                // find the required extended property
                var prop = extProps.FirstOrDefault(p => p.Name == bprop.ExtendedPropertyName && p.TemplateType == template.TemplateType);
                updateDto.PersonExtendedPropertyList.Add(new WriteEntityExtendedPropertyDto()
                {
                    ExtendedPropertyId = prop.Id,
                    Value = bprop.Value
                });
            }

            return updateDto;
        }

        private static AddUserDto BuildAddUserDto(BuilderAddUserDto bAddDto, IList<ExtendedPropertyDetailDto> extProps, IList<TemplateDetailDto> templates, IList<RoleSummaryDto> roles)
        {
            var addDto = new AddUserDto()
            {
                GivenName = bAddDto.GivenName,
                Surname = bAddDto.Surname,
                Email = bAddDto.Email,
                Password = bAddDto.Password
            };

            // find the item template with the desired name
            var template = templates.FirstOrDefault(t => t.Name == bAddDto.TemplateName && t.TemplateType == TemplateFor.Person);
            if (template == null)
                throw new InvalidOperationException("Template not found for adding person");
            addDto.TemplateId = template.Id;

            // find the role with the desired name
            var role = roles.FirstOrDefault(t => t.Name == bAddDto.RoleName);
            if (role == null)
                throw new InvalidOperationException("Role not found for adding user");
            addDto.RoleId = role.Id;

            // add all extended properties
            foreach (var bprop in bAddDto.BuilderPersonExtendedPropertyList)
            {
                // find the required extended property
                var prop = extProps.FirstOrDefault(p => p.Name == bprop.ExtendedPropertyName && p.TemplateType == template.TemplateType);
                addDto.PersonExtendedPropertyList.Add(new WriteEntityExtendedPropertyDto()
                {
                    ExtendedPropertyId = prop.Id,
                    Value = bprop.Value
                });
            }

            // add all tags
            foreach (var tag in bAddDto.PersonTagList)
            {
                addDto.PersonTagList.Add(tag);
            }

            return addDto;
        }

        private static UpdateItemDto BuildUpdateItemDto(Guid Id, BuilderAddItemDto bAddDto, IList<ExtendedPropertyDetailDto> extProps, IList<SkuDetailDto> skus, IList<PersonDetailDto> persons)
        {
            var updateDto = new UpdateItemDto()
            {
                Id = Id,
                Name = bAddDto.Name,
                Description = bAddDto.Description,
            };

            // find the sku with the desired name
            var sku = skus.FirstOrDefault(t => t.Name == bAddDto.SkuName);
            if (sku == null)
                throw new InvalidOperationException("Sku not found for updating item");
            updateDto.SkuId = sku.Id;

            // find the person with the desired name
            var person = persons.FirstOrDefault(t => t.Email == bAddDto.PersonEmail);
            if (person == null)
                throw new InvalidOperationException("Person not found for updating item");
            updateDto.PersonId = person.Id;

            foreach (var bprop in bAddDto.BuilderItemExtendedPropertyList)
            {
                // find the required extended property
                var prop = extProps.FirstOrDefault(p => p.Name == bprop.ExtendedPropertyName && p.TemplateType == TemplateFor.Item);
                updateDto.ItemExtendedPropertyList.Add(new WriteItemExtendedPropertyDto()
                {
                    ExtendedPropertyId = prop.Id,
                    Value = bprop.Value
                });
            }

            return updateDto;
        }

        private static UpdateItemTagDto BuildUpdateItemTagDto(Guid Id, BuilderAddItemDto bAddDto)
        {
            var updateDto = new UpdateItemTagDto()
            {
                Id = Id
            };

            foreach (var btag in bAddDto.ItemTagList)
            {
                updateDto.ItemTagList.Add(btag);
            }

            return updateDto;
        }

        private static UpdateItemPlaceDto BuildUpdateItemPlaceDto(Guid Id, BuilderAddItemDto bAddDto, IList<PlaceDetailDto> places)
        {
            var updateDto = new UpdateItemPlaceDto()
            {
                Id = Id
            };

            // find the new place with the desired name
            var place = places.FirstOrDefault(t => t.Name == bAddDto.PlaceName);
            if (place == null)
                throw new InvalidOperationException("Place not found for updating item");
            updateDto.NewPlaceId = place.Id;

            return updateDto;
        }

        private static AddItemDto BuildAddItemDto(BuilderAddItemDto bAddDto, IList<ExtendedPropertyDetailDto> extProps, IList<SkuDetailDto> skus, IList<PersonDetailDto> persons, IList<PlaceDetailDto> places)
        {
            var addDto = new AddItemDto()
            {
                Name = bAddDto.Name,
                Description = bAddDto.Description
            };

            // find the sku with the desired name
            var sku = skus.FirstOrDefault(t => t.Name == bAddDto.SkuName);
            if (sku == null)
                throw new InvalidOperationException("Sku not found for adding item");
            addDto.SkuId = sku.Id;

            // find the person with the desired name
            var person = persons.FirstOrDefault(t => t.Email == bAddDto.PersonEmail);
            if (person == null)
                throw new InvalidOperationException("Person not found for adding item");
            addDto.PersonId = person.Id;

            // find the place with the desired name
            var place = places.FirstOrDefault(t => t.Name == bAddDto.PlaceName);
            if (place == null)
                throw new InvalidOperationException("Place not found for adding item");
            addDto.PlaceId = place.Id;

            // add all extended properties
            foreach (var bprop in bAddDto.BuilderItemExtendedPropertyList)
            {
                // find the required extended property
                var prop = extProps.FirstOrDefault(p => p.Name == bprop.ExtendedPropertyName && p.TemplateType == TemplateFor.Item);
                addDto.ItemExtendedPropertyList.Add(new WriteItemExtendedPropertyDto()
                {
                    ExtendedPropertyId = prop.Id,
                    Value = bprop.Value
                });
            }

            // add all tags
            foreach (var tag in bAddDto.ItemTagList)
            {
                addDto.ItemTagList.Add(tag);
            }

            return addDto;
        }
        #endregion
    }
}
